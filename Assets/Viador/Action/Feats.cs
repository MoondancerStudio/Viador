using UnityEngine;


namespace Assets.Viador.Action
{
    public enum FeatType
    {
        Attack,
        Defense,
        Run
    }
    public class Feats
    {
        public string name { get; private set; }
        public int actionValue { get; private set; }
        public int cost { get; private set; }

        public FeatType featType { get; private set; }

        public bool isPayed { get; set; }

        public Feats(string name, int actionValue, int cost, FeatType featType)
        {
            this.name = name;
            this.actionValue = actionValue;
            this.cost = cost;
            this.isPayed = false;
            this.featType = featType;
        }

        public void buyActionState()
        {
            isPayed = true;
        }

        public void deActiveState()
        {
            isPayed = false;
        }
    }
}
