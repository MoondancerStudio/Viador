using System;
using UnityEngine;
using UnityEngine.Events;

namespace Viador.Events
{
   public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private string gameEventName;
        [SerializeField] private string AttackgameEventName;
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private GameEvent attackgameEvent;

        [SerializeField] private CustomGameEvent response;

        private void Awake()
        {
            //Debug.Log($"[{gameObject.name}] awaken");
            if (gameEvent is null && gameEventName is not null)
            {
                //Debug.Log($"[{gameObject.name}] getting game event {gameEventName}");
                gameEvent = GameEventProvider.Get(gameEventName);
            }

            if (attackgameEvent is null && AttackgameEventName is not null)
            {
                //Debug.Log($"[{gameObject.name}] getting game event {gameEventName}");
                attackgameEvent = GameEventProvider.Get(AttackgameEventName);
            }
        }

        private void OnEnable()
        {
            gameEvent.Subscribe(this);
            attackgameEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            gameEvent.Unsubscribe(this);
            attackgameEvent.Unsubscribe(this);  
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