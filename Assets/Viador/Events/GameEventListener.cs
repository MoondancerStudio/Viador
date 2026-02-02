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
            //GameLogger.Log(LoggerType.GAME_INFO,$"[{gameObject.name}] awaken");
            if (gameEvent is null && gameEventName is not null)
            {
                //GameLogger.Log(LoggerType.GAME_INFO,$"[{gameObject.name}] getting game event {gameEventName}");
                gameEvent = GameEventProvider.Get(gameEventName);
            }
        }

        private void OnEnable()
        {
            gameEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            gameEvent.Unsubscribe(this);
        }

        public void OnEventTriggered(Component sender, object data)
        {
            //GameLogger.Log(LoggerType.GAME_INFO,$"[{sender?.name}] triggered [{gameEvent?.name}], handled in [{gameObject.name}]");
            response.Invoke(sender, data);
        }
    }
   
    [Serializable]
    public class CustomGameEvent : UnityEvent<Component, object>
    {
    }
}