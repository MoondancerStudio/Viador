using System;
using UnityEngine;
using UnityEngine.Events;

namespace Viador.Events
{
   public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent moveEvent;
        [SerializeField] private GameEvent attackEvent;

        [SerializeField] private CustomGameEvent response;

        private void OnEnable()
        {
            moveEvent.Subscribe(this);
            attackEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            moveEvent.Unsubscribe(this);
            attackEvent.Unsubscribe(this);
        }

        public void OnEventTriggered(Component sender, object data)
        {
            // Debug.Log($"[{sender.name}] triggered [{gameEvent.name}], handled in [{gameObject.name}]");
            response.Invoke(sender, data);
        }
    }
   
    [Serializable]
    public class CustomGameEvent : UnityEvent<Component, object>
    {
    }
}