using System.Collections.Generic;
using UnityEngine;

namespace Viador.Events
{
    public static class GameEventProvider
    {
        private static Dictionary<string, GameEvent> _gameEvents;

        public static GameEvent Get(string eventName)
        {
            Init();

            GameEvent gameEvent = _gameEvents.TryGetValue(eventName, out var @event) ? @event : Create(eventName);
            //Debug.Log($"GameEventProvider.Get => {gameEvent.name}");
            return gameEvent;
        }

        private static void Init()
        {
            if (_gameEvents == null)
            {
                _gameEvents = new Dictionary<string, GameEvent>();
                foreach (var gameEvent in Resources.FindObjectsOfTypeAll<GameEvent>())
                {
                    _gameEvents.Add(gameEvent.name, gameEvent);
                }
            }
        }
        
        private static GameEvent Create(string eventName)
        {
            GameEvent gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.name = eventName;
            _gameEvents.Add(eventName, gameEvent);

            return gameEvent;
        }
    }
}