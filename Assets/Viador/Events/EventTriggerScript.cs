using UnityEngine;

namespace Viador.Events
{
    public class EventTriggerScript : MonoBehaviour
    {
        [SerializeField] private string gameEventName;
        [SerializeField] private GameEvent gameEvent;

        private void Awake()
        {
            if (gameEventName is not null)
            {
                gameEvent = GameEventProvider.Get(gameEventName);
            }
        }

        public void FireGameEvent()
        {
            gameEvent.Trigger(this, null);
        }
    }
}
