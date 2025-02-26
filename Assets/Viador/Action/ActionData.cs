using UnityEngine;

namespace Viador.Action
{
    [CreateAssetMenu(menuName = "Action Data")]
    public class ActionData : ScriptableObject
    {
        public new string name;
        public int cost;
        public Sprite icon;
        public string description;
    }
}
