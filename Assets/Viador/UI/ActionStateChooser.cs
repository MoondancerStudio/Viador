using Viador.Events;
using Viador.Action;
using UnityEngine;
using System.Threading;
using Assets.Viador.Action;

namespace Viador.UI
{
    public class ActionStateChooser : MonoBehaviour
    {
        public int cost = 2;

        public void OnActionStateClicked()
        {
            GameEventProvider.Get(GameEvents.OnPurchaseActionState).Trigger(this, new Feats(name, 3, cost));
        }
    }
}