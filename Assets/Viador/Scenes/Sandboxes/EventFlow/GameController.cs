using System.Collections.Generic;
using UnityEngine;
using Viador.Events;

namespace Viador.Scenes.Sandboxes.EventFlow
{
    public class GameController : MonoBehaviour
    {
        private TurnManager _turnManager;
        [SerializeField] private List<string> players;

        private void Awake()
        {
            _turnManager = new TurnManager(players);
            Debug.Log($"[{gameObject.name}] awaken");
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
    }
}
