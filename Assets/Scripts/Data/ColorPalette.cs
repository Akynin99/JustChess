using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "JustChess/ColorPalette", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private Color[] colors;

        public Color[] Colors => colors;

        public Color GetColorByIndex(int index)
        {
            if (colors.Length <= index) return colors[^1];

            return colors[index];
        }
    }
}