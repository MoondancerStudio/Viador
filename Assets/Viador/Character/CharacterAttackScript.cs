using System;
using UnityEngine;
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

        private CombatLogic _combatLogic = new();

        void Awake()
        {
            _includeLayer = LayerMask.GetMask("AttackHighlight");
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
        
        // Executing attack
        public void OnCharacterDamaged(Component sender, object baseDefenseValueRaw)
        {
            if (TurnManager._currentPlayer.Equals(name))
            {
                int baseDefenseValue = int.Parse(baseDefenseValueRaw.ToString());

                AttackResult attackResult = _combatLogic.CalculateAttack(_combatLogic.CalculateAttackValue(characterData), baseDefenseValue, new Dice());

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
            if (sender.name.Equals(name))
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
