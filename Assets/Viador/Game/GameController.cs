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
        public List<CharacterData> CharacterData
        {
            get { return characterData; }
            private set { characterData = value; }
        }

        private void Awake()
        {
            List<string> _players = new ();
            characterData.ForEach(character =>
                {
                    _players.Add(character.name);
                }
            );
            
            _turnManager = new TurnManager(_players);
            GameLogger.Log(LoggerType.TURN, $"[{gameObject.name}] awaken");

            GameEventProvider.Get(GameEvents.Player_2_HealthPointUpdated).Trigger(this, characterData[0].health);
            GameEventProvider.Get(GameEvents.Player_1_HealthPointUpdated).Trigger(this, characterData[1].health);
        }

        private void Start()
        {
            GameLogger.Log(LoggerType.TURN, $"[{gameObject.name}] starting");
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

        public void OnPurchaseActionState(Component caller, object payload)
        {
            GameLogger.Log(LoggerType.FEATS, $"Bought Action: {(int)payload}");
            if (payload is int)
            {
                _turnManager.OnPurchaseActionState((int)payload);
            }
        }
    }
}
