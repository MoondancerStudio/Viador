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
        private GameEventListener _gameEventListener;


        private void Awake()
        {
            gameObject.name = characterData.name;
            gameObject.GetComponent<SpriteRenderer>().sprite = characterData.icon;
        }
    }
}
