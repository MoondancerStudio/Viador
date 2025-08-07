
using UnityEngine;
using Assets.Viador.Action;
using System.Collections.Generic;
using System.Linq;

namespace Viador.Action
{
    public interface IActionStateProvider
    {
        public void addNewActionState(Feats actionState);
        public void Execute(string actionStateName);
        public int getActionStateDefense();
        public int getActionStateAttack();
        public bool isActionStateListEmpty();

        public void removeAllActivatedActionStates();
        public bool hasFeat(Feats feat);
    }

    public class ActionStateProvider : IActionStateProvider
    {
        private List<Feats> feats = new();

        public void addNewActionState(Feats actionState)
        {
            feats.Add(actionState);
        }

        public void Execute(string actionStateName)
        {
            var f = feats.Find(x => x.name == actionStateName);


            if (f != null)
            {
                Debug.Log("feat is: " + f.name);
                f.buyActionState();
            }
        }

        public bool isActionStateListEmpty()
        {
            return feats.Count == 0;   
        }

        public int getActionStateAttack()
        {
            if(isActionStateListEmpty())
                return 0;

            List<Feats> getAllActivatedActionState = feats.FindAll(x => x.isPayed);

            return getAllActivatedActionState.Sum((x) => x.actionValue);
        }

        public int getActionStateDefense()
        {
            return 0;
        }

        public void removeAllActivatedActionStates()
        {
            feats.RemoveAll(x => x.isPayed);
        }

        public bool hasFeat(Feats feat)
        {
            return feats.Any(f => f.name == feat.name);
        }
    }
}