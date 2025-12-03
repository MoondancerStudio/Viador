
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
        public bool isFeatActivated(string feat);
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
            var f = feats.FirstOrDefault(x => x.name == actionStateName);

            if (f != null)
            {
                Debug.Log("feat is: " + f.name + " payment:" + f.isPayed);
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

            List<Feats> getAllActivatedActionState = feats.FindAll(x => x.featType == FeatType.Attack);

            return getAllActivatedActionState.Sum((x) => x.actionValue);
        }

        public int getActionStateDefense()
        {
            if (isActionStateListEmpty())
                return 0;

            List<Feats> getAllActivatedActionState = feats.FindAll(x => x.featType == FeatType.Defense);

            return getAllActivatedActionState.Sum((x) => x.actionValue);
        }

        public void removeAllActivatedActionStates()
        {
            feats.FindAll(x => x.isPayed).ForEach(x => x.deActiveState());
        }

        public bool hasFeat(Feats feat)
        {
            return feats.Any(f => f.name == feat.name);
        }

        public bool isFeatActivated(string feat)
        {
            var f = feats.FirstOrDefault(x=> x.name == feat);

            if(f != null)
            {
                Debug.Log("Feats payed: " + f.isPayed);
                return f.isPayed;
            }
            return true;
        }
    }
}