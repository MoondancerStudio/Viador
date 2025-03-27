using System;
using UnityEditor.U2D.Animation;
using UnityEngine;
using Viador.Events;
using Viador.Game;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterAttackScript : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = Vector2.one;

        private LayerMask _includeLayer;

        public CharacterData characterData;

        void Awake()
        {
            _includeLayer = LayerMask.GetMask("AttackHighlight");
        }

        void Start()
        {
            if (characterData is null)
            {
                throw new NullReferenceException("No Character data found");
            }
        }

        public void OnCharacterDamaged(Component sender, object hp)
        {
            if (TurnManager.activePlayer.Equals(name))
            {
                Debug.Log($"[Sender]: {sender} [Health points of player 2]: {hp}");

                int calculateDamage = (int)hp - characterData.attack;

                Debug.Log($"[Sender's attack power is]: {characterData.attack} [Remaining health points of player 2 is]: {calculateDamage}");

                GameEventProvider.Get(GameEvents.CharacterDefensed).Trigger(sender, calculateDamage);
            }
        }

        public void OnCharacterDefensed(Component sender, object healthLost)
        {
            if (sender.name.Equals(name))
            {
                Debug.Log($"[Sender]: {sender} [Health points lost of player 2]: {healthLost}");
                int updateHP = characterData.defense + (int)healthLost;

                Debug.Log($"[player 2 defense is]: {characterData.defense}, so the health points is: {updateHP}");

                // Update hp
                characterData.health = updateHP;

                // Send to UI
                GameEventProvider.Get(GameEvents.UIStateUpdated).Trigger(sender, updateHP);
            }
        }

        private void OnMouseDown()
        {
            if (Physics2D.OverlapBox(transform.position, sizeOfBoxCollider, 0, _includeLayer))
            {
                Debug.Log($"Start attack on {name}");

                GameEventProvider.Get(GameEvents.CharacterChoosenToAttack).
                    Trigger(this, characterData.health);
                GameEventProvider.Get(GameEvents.CharacterMoved).Trigger(this, null);
            }
        }
    }
}
