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

        private CombatLogic _underTest = new();

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

        public float calculateAttack(CharacterData enemyStat)
        {
            // Dice rolls
            int d10_1 = UnityEngine.Random.Range(1, enemyStat.attack);
            int d10_2 = UnityEngine.Random.Range(1, enemyStat.attack);

            float damageScore = (characterData.attack + d10_1) - (enemyStat.defense + d10_2);
         
            if (damageScore > 0)
            {
                return damageScore / 2;
            }
            return 0;
        }

        public void OnCharacterDamaged(Component sender, object hp)
        {
            if (TurnManager._currentPlayer.Equals(name))
            {
                float enemyHealthPoint = float.Parse(hp.ToString());
                Debug.Log($"[Attacker name]: {name} [Health points of {sender.name}]: {enemyHealthPoint.ToString()}");

                CharacterData DefenderCharacterData = (sender as CharacterAttackScript).characterData;

                AttackResult attackResult = _underTest.CalculateAttack(characterData.attack, DefenderCharacterData.defense, new Dice());
                float calculateDamage = enemyHealthPoint - attackResult.Damage;

                Debug.Log($"[Attacker's attack power is]: {characterData.attack} and the [random damaga score]: {attackResult.Damage} [Remaining health points of player 2 is]: {calculateDamage}");

                GameEventProvider.Get(GameEvents.CharacterDefensed).Trigger(sender, calculateDamage);


                string attackResultMessage = attackResult.Success ? $" -{attackResult.Damage} hp" : "Miss";

                GameEventProvider.Get(GameEvents.AttackResultUpdated).Trigger(this, attackResultMessage);
            }
        }

        public void OnCharacterDefensed(Component sender, object healthLost)
        {
            if (sender.name.Equals(name))
            {
                float enemyHealthPoint = float.Parse(healthLost.ToString());
                Debug.Log($"[Attacker]: {sender.name} [Health points lost of player {name}]: {enemyHealthPoint}");
                float updateHP =  enemyHealthPoint;

                Debug.Log($"{name}'s defense is]: {characterData.defense}, so the health points is: {updateHP}");

                // Update hp
                characterData.health = (int)updateHP;

                // Send to UI
                if (updateHP <= 0)
                {
                    updateHP = 0;
                    GameEventProvider.Get(GameEvents.GameOver).Trigger(this, null);
                }


                if (name == "Dracon")
                    GameEventProvider.Get(GameEvents.Player_1_HealthPointUpdated).Trigger(this, updateHP);
                else
                    GameEventProvider.Get(GameEvents.Player_2_HealthPointUpdated).Trigger(this, updateHP);
            }
        }

        private void OnMouseDown()
        {
            Debug.Log($"Current player is: {TurnManager._currentPlayer}");
            if (Physics2D.OverlapBox(transform.position, sizeOfBoxCollider, 0, _includeLayer))
            {
                Debug.Log($"Start attack on {name}");

                _gridController.ResetHighlight();

                GameEventProvider.Get(GameEvents.CharacterChoosenToAttack).
                    Trigger(this, characterData.health);
                GameEventProvider.Get(GameEvents.CharacterMoved).Trigger(this, null);

            }
        }
    }
}
