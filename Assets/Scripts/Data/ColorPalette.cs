using UnityEngine;
using UnityEngine.Serialization;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "JustChess/ColorPalette", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        [FormerlySerializedAs("colors")] [SerializeField] private Color[] mainColors;
        [FormerlySerializedAs("chooseColor")] [SerializeField] private Color highlightSquareColor;

        public Color[] MainColors => mainColors;
        public Color HighlightSquareColor => highlightSquareColor;

        public Color GetColorByIndex(int index)
        {
            if (mainColors.Length <= index) return mainColors[^1];

            return mainColors[index];
        }
    }
}