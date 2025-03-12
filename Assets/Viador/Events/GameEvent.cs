using System.Collections.Generic;
using UnityEngine;

namespace Viador.Events
{
    [CreateAssetMenu(menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new();

        public void Trigger(Component sender, object data)
        {
            // Debug.Log($"Triggered ({_listeners.Count} listeners)");
            for (var index = _listeners.Count - 1; index >= 0; index--)
            {
                var listener = _listeners[index];
                listener.OnEventTriggered(sender, data);
            }
        }

        public void Subscribe(GameEventListener listener)
        {
            if (!_listeners.Contains(listener)) _listeners.Add(listener);
        }

        public void Unsubscribe(GameEventListener listener)
        {
            if (_listeners.Contains(listener)) _listeners.Remove(listener);
        }
    }
}