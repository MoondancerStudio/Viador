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
            name = characterData.name;
            GetComponent<SpriteRenderer>().sprite = characterData.icon;
            GetComponent<CharacterAttackScript>().characterData = characterData;
        }
    }
}
