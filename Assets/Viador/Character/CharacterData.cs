using UnityEngine;

namespace Viador.Character
{
    [CreateAssetMenu(menuName = "Character Data")]
    public class CharacterData : ScriptableObject
    {
        public new string name;
        public Sprite icon;
        public Sprite image;
        public string description;

        public int move;
        public int attack;
        public int defense;
        public int health;
        public int armor;
        public int[] armorStructure;
    }
}
