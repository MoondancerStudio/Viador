using System;
using UnityEngine;
using UnityEngine.Events;

namespace Viador.Events
{
   public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private string gameEventName;
        [SerializeField] private GameEvent gameEvent;

        [SerializeField] private CustomGameEvent response;

        private void Awake()
        {
            //Debug.Log($"[{gameObject.name}] awaken");
            if (gameEvent is null && gameEventName is not null)
            {
                //Debug.Log($"[{gameObject.name}] getting game event {gameEventName}");
                gameEvent = GameEventProvider.Get(gameEventName);
            }
        }

        private void OnEnable()
        {
            //Debug.Log($"[{gameObject.name}] enabled");
            gameEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            gameEvent.Unsubscribe(this);
        }

        public void OnEventTriggered(Component sender, object data)
        {
            //Debug.Log($"[{sender?.name}] triggered [{gameEvent?.name}], handled in [{gameObject.name}]");
            response.Invoke(sender, data);
        }
    }
   
    [Serializable]
    public class CustomGameEvent : UnityEvent<Component, object>
    {
    }
}