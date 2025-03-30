using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Viador.Events;
using Viador.Game;

namespace Viador.Character
{
    public class CharacterScript : MonoBehaviour
    {
        private static readonly Vector2 sizeOfBoxCollider = Vector2.one;

        [SerializeField] private CharacterData characterData;
        private GameEventListener _gameEventListener;



        private void Awake()
        {
            gameObject.name = characterData.name;
            gameObject.GetComponent<SpriteRenderer>().sprite = characterData.icon;
          
            _gameEventListener = gameObject.GetComponent<GameEventListener>();

            if (_gameEventListener == null) 
            {
                throw new NullReferenceException("No event listener found");
            }
        }

        // Player 1 megt�madja player 2-t, ezt most Player 1-n�l van
        public void OnCharacterDamaged(Component sender, object hp)
        {
            Debug.Log($"[Sender]: {sender} [Health points of player 2]: {hp}");

            int calculateDamage = (int)hp - characterData.attack;

            Debug.Log($"[Sender's attack power is]: {characterData.attack} [Remaining health points of player 2 is]: {calculateDamage}");
         
            GameEventProvider.Get(GameEvents.CharacterDefensed).Trigger(this, calculateDamage);
        }

        // T�mad�s ut�n megkapja a player 2 a hp vesztess�get �s
        // korrig�lja v�dekez�ssel
        public void OnCharacterDefensed(Component sender, object healthLost)
        {
            Debug.Log($"[Sender]: {sender} [Health points lost of player 2]: {healthLost}");
            int updateHP = characterData.defense + (int)healthLost;

            Debug.Log($"[player 2 defense is]: {characterData.defense}, so the health points is: {updateHP}");

            // Update hp
            characterData.health = updateHP;

            // Send to UI
            GameEventProvider.Get(GameEvents.UIStateUpdated).Trigger(this, updateHP);
        }

        // A v�dekez�s ut�n kik�ldj�k a v�gleges �leter�t player 2 �nmag�nak,
        // majd a UI-nak is majd meg egy�b helyekre, ahol fontos
        public void OnHealthPointUpdated(Component sender, object hp) 
        {
            // TODO: UI update!
        }

        private void OnMouseDown()
        {
            if (Physics2D.OverlapBox(transform.position, sizeOfBoxCollider, 0, LayerMask.GetMask("AttackHighlight")))
            {
                Debug.Log($"Start attack on {name}");

                _gameEventListener.enabled = true;
                GameEventProvider.Get(GameEvents.CharacterChoosenToAttack).
                    Trigger(this, characterData.health);
                _gameEventListener.enabled = false;
            }
        }
    }
}
