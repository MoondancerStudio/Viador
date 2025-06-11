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
        [SerializeField] private CharacterData characterData;

        private void Awake()
        {
            gameObject.name = characterData.name;
            gameObject.GetComponent<SpriteRenderer>().sprite = characterData.icon;
            gameObject.GetComponent<CharacterAttackScript>()
                .characterData = new CharacterData
                    { 
                        health = characterData.health,
                        attack = characterData.attack,
                        defense = characterData.defense,
                    };

            GameEventProvider.Get(GameEvents.UIStateUpdated).Trigger(this, characterData.health);
        }
    }
}
