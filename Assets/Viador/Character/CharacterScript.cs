using System;
using UnityEngine;

namespace Viador.Character
{
    public class CharacterScript : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;

        public int _health;

        private void Awake()
        {
            gameObject.name = characterData.name;
            gameObject.GetComponent<SpriteRenderer>().sprite = characterData.icon;
            _health = characterData.health;
        }
    }
}
