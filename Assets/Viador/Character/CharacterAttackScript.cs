using Assets.Viador.Action;
using System;
using System.Collections.Generic;
using UnityEngine;
using Viador.Action;
using Viador.Events;
using Viador.Game;
using Viador.GameMechanics;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterAttackScript : MonoBehaviour
    {
        [SerializeField] private GridController _gridController;

        private static readonly Vector2 sizeOfBoxCollider = new Vector2(0.3f,0.3f);

        private LayerMask _includeLayer;

        public CharacterData characterData;

        private IActionStateProvider _actionStateProvider;

        private CombatLogic _combatLogic = new();

        void Awake()
        {
            _includeLayer = LayerMask.GetMask("AttackHighlight");
            _actionStateProvider = new ActionStateProvider();
            _actionStateProvider.addNewActionState(new Feats("Harci Laz", 3, 2, FeatType.Attack));
            _actionStateProvider.addNewActionState(new Feats("Piszkos Csel", 3, 1, FeatType.Attack));
            _actionStateProvider.addNewActionState(new Feats("Futas", 2, 1, FeatType.Run));
            _actionStateProvider.addNewActionState(new Feats("Kiteres", 2, 1, FeatType.Defense));
        }

        void Start()
        {
            _gridController = GameObject.Find("Grid").GetComponent<GridController>();

            if (_gridController is null)
            {
                throw new NullReferenceException("No GridController found");
            }

            if (characterData is null)
            {
                throw new NullReferenceException("No Character data found");
            }

        }

        public void OnRunFeatSelectedEvent()
        { 
            Debug.Log($"Run action state On");

            GameEventProvider.Get(GameEvents.OnRunFeatHighLight).Trigger(this, transform.position);
        }

        /**
         * Select target
         */
        private void OnMouseDown()
        {
            Debug.Log($"Current player is: {TurnManager._currentPlayer}");
            if (Physics2D.OverlapBox(transform.position, sizeOfBoxCollider, 0, _includeLayer))
            {
                Debug.Log($"Start attack on {name}");

                _gridController.ResetHighlight();

                // Check if the attack has already been launched by someone
                // Set it true if nobodoy has started the fight
                if(!CombatLogic.IsAttackBegin)
                    CombatLogic.IsAttackBegin = true;

                // Trigger attack calculation
                GameEventProvider.Get(GameEvents.CharacterChoosenToAttack).
                    Trigger(this, _combatLogic.CalculateDefenseValue(characterData));
                
                // Trigger action point update
                GameEventProvider.Get(GameEvents.CharacterAttacked).Trigger(this, null);
            }
        }

         /**
          * Purchase feat
          */
        public void OnPurchaseActionState(Component sender, object actionState)
        {
            if (TurnManager._currentPlayer != name)
                return;

            if (_actionStateProvider.isActionStateListEmpty())
            {
                Debug.Log("CAN NOT BUY ACTION STATE!");
            } 
            else
            {
                if (actionState is Feats feats)
                {
                    if (!_actionStateProvider.hasFeat(feats))
                    {
                        Debug.Log("Feat doest not exist!");
                        return;
                    }

                    if (_actionStateProvider.isFeatActivated(feats.name))
                    {
                        Debug.Log("Feat is already in use!");
                        return;
                    }

                    Debug.Log($"Get Action state: {feats.name} with bonus attack value {feats.actionValue} costs {feats.cost}");
                    
                    if (!TurnManager.isEnoughActionPointsForPurchase(feats.cost))
                        return;

                    _actionStateProvider.Execute(feats.name);
                    GameEventProvider.Get(GameEvents.OnUpdatePurchasedActionState).Trigger(this, feats.cost);

                    if (feats.featType == FeatType.Run)
                    {
                        OnRunFeatSelectedEvent();
                    }
                   // _actionStateProvider.removeAllActivatedActionStates();
                }
            }
        }
        
        // Executing attack
        public void OnCharacterDamaged(Component sender, object baseDefenseValueRaw)
        {
            if (TurnManager._currentPlayer.Equals(name))
            {
                int baseDefenseValue = int.Parse(baseDefenseValueRaw.ToString());

                AttackResult attackResult = _combatLogic.CalculateAttack(
                    _combatLogic.CalculateAttackValue(characterData),
                    baseDefenseValue, new Dice(), _actionStateProvider
                );

                ShowAttackResult(attackResult);

                HandleAttackResult(sender, attackResult);
            }
        }

        private void ShowAttackResult(AttackResult attackResult)
        {
            var message = attackResult.Success ? "Hit" : "Miss";
            GameEventProvider.Get(GameEvents.AttackResultUpdated).Trigger(this, message);
        }

        private void HandleAttackResult(Component sender, AttackResult attackResult)
        {
            if (attackResult.Success)
            {
                Debug.Log($"{name} attacked successfully with a raw damage of {attackResult.Damage}");
                GameEventProvider.Get(GameEvents.CharacterDefensed).Trigger(sender, attackResult.Damage);
            }
            else
            {
                Debug.Log($"{name} missed the attack");
            }
        }

        public void OnCharacterDefensed(Component sender, object rawDamageRaw)
        {
            if (sender.name == name)
            {
                int rawDamage = int.Parse(rawDamageRaw.ToString());
                int damage = _combatLogic.HandleDamage(rawDamage, characterData.armor);
                
                Debug.Log($"{name} received {damage} effective damage");
                Debug.Log($"{name} has {characterData.health} health");

                // Update hp
                var newHealth = characterData.health - damage;
                characterData.health = newHealth;

                Debug.Log($"{name} new health {characterData.health}");

                GameEventProvider.Get(GameEvents.AttackResultUpdated).Trigger(this, $"Hit (-{damage} hp)");

                UpdateHpHighlight();
                HandleCharacterDeath();
            }
        }

        private void UpdateHpHighlight()
        {
            Debug.Log($"Health {characterData.health}");
            string eventToTrigger;
                    
            if (name == "Dracon")
            {
                eventToTrigger = GameEvents.Player_1_HealthPointUpdated;
            }
            else
            {
                eventToTrigger = GameEvents.Player_2_HealthPointUpdated;
            }

            GameEventProvider.Get(eventToTrigger).Trigger(this, characterData.health);
        }
        
        private void HandleCharacterDeath()
        {
            if (characterData.health <= 0)
            {
                characterData.health = 0;
                                                                                     GameEventProvider.Get(GameEvents.GameOver).Trigger(this, null);
            }
        }
    }
}
