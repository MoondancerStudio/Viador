using Viador.Events;
using Viador.Action;
using UnityEngine;
using System.Threading;
using Assets.Viador.Action;

namespace Viador.UI
{
    public class ActionStateChooser : MonoBehaviour
    {
        [SerializeField] public new string name;
        [SerializeField] public int cost;
        [SerializeField] public int value;

        public void OnActionStateClicked()
        {
            GameEventProvider.Get(GameEvents.OnPurchaseActionState).Trigger(this, new Feats(name, value, cost));
            
        }
    }
}