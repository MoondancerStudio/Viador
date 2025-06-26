using UnityEngine;

namespace Assets.Viador.Action
{
    public class Feats
    {
        public string name { get; private set; }
        public int actionValue { get; private set; }
        public int cost { get; private set; }

        public bool isPayed { get; private set; }

        public Feats(string name, int actionValue, int cost)
        {
            this.name = name;
            this.actionValue = actionValue;
            this.cost = cost;
            this.isPayed = false;
        }

        public void buyActionState()
        {
            isPayed = true;
        }
    }
}
