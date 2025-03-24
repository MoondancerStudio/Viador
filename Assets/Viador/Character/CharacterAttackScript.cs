using System;
using UnityEditor.U2D.Animation;
using UnityEngine;
using Viador.Events;
using Viador.Map;

namespace Viador.Character
{
    public class CharacterAttackScript : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = Vector2.one;

        [SerializeField] private CharacterData _characterData;
        [SerializeField] private GameEventListener _gameEventListener;
        private LayerMask _includeLayer;

        void Awake()
        {
            _includeLayer = LayerMask.GetMask("AttackHighlight");

            if (_characterData is null)
            {
                throw new NullReferenceException("No Character data found");
            }
        }

        // Player 1 megtámadja player 2-t, ezt most Player 1-nél van
        public void OnCharacterDamaged(Component sender, object hp)
        {
            Debug.Log($"[Sender]: {sender} [Health points of player 2]: {hp}");

            int calculateDamage = (int)hp - _characterData.attack;

            Debug.Log($"[Sender's attack power is]: {_characterData.attack} [Remaining health points of player 2 is]: {calculateDamage}");

            GameEventProvider.Get(GameEvents.CharacterDefensed).Trigger(this, calculateDamage);
        }

        // Támadás után megkapja a player 2 a hp vesztességet és
        // korrigálja védekezéssel
        public void OnCharacterDefensed(Component sender, object healthLost)
        {
            Debug.Log($"[Sender]: {sender} [Health points lost of player 2]: {healthLost}");
            int updateHP = _characterData.defense + (int)healthLost;

            Debug.Log($"[player 2 defense is]: {_characterData.defense}, so the health points is: {updateHP}");

            // Update hp
            _characterData.health = updateHP;

            // Send to UI
            GameEventProvider.Get(GameEvents.UIStateUpdated).Trigger(this, updateHP);
        }

        // A védekezés után kiküldjük a végleges életerőt player 2 önmagának,
        // majd a UI-nak is majd meg egyéb helyekre, ahol fontos
        public void OnHealthPointUpdated(Component sender, object hp)
        {
            // TODO: UI update!
        }

        private void OnMouseDown()
        {
            if (Physics2D.OverlapBox(transform.position, sizeOfBoxCollider, 0, _includeLayer))
            {
                Debug.Log($"Start attack on {name}");

                _gameEventListener.enabled = true;
                GameEventProvider.Get(GameEvents.CharacterChoosenToAttack).
                    Trigger(this, _characterData.health);
                GameEventProvider.Get(GameEvents.CharacterMoved).Trigger(this, null);
                _gameEventListener.enabled = false;

            }
        }
    }
}
