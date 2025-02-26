using UnityEngine;

namespace Viador
{
    [CreateAssetMenu(menuName = "Color Palette")]
    public class ColorPalette : ScriptableObject
    {
        public Color white;
        public Color black;

        public Color primary;
        public Color primary2;
        public Color primary3;

        public Color secondary;
        public Color secondary2;
        public Color secondary3;

        public Color highlight;
    }
}
