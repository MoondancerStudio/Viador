using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Viador.Character;
using Viador.Events;

namespace Viador.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> characterData;
        private TurnManager _turnManager;

        private void Awake()
        {
            List<string> _players = new ();
            characterData.ForEach(character =>
                {
                    _players.Add(character.name);
                }
            );
            
            _turnManager = new TurnManager(_players);
            Debug.Log($"[{gameObject.name}] awaken");

            GameEventProvider.Get(GameEvents.Player_2_HealthPointUpdated).Trigger(this, characterData[0].health);
            GameEventProvider.Get(GameEvents.Player_1_HealthPointUpdated).Trigger(this, characterData[1].health);
        }

        private void Start()
        {
            Debug.Log($"[{gameObject.name}] starting");
            GameEventProvider.Get(GameEvents.StartGame).Trigger(this, null);
            GameEventProvider.Get(GameEvents.NextTurn).Trigger(this, null);
        }

        public void OnNextTurn(Component caller, object payload)
        {
            _turnManager.OnNextTurn();
        }

        public void OnMoved(Component caller, object payload)
        {
            _turnManager.OnMoved();
        }

        public void OnAttacked(Component caller, object payload)
        {
            _turnManager.OnAttacked();
        }
    }
}
