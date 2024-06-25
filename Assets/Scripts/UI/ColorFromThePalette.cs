using System;
using JustChess.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JustChess.UI
{
    public class ColorFromThePalette : MonoBehaviour
    {
        [SerializeField, Range(0, 4)] private int colorIndex;

        [Inject] private ColorPalette _colorPalette;

        private void Awake()
        {
            Image image = GetComponent<Image>();

            if (image)
            {
                image.color = _colorPalette.GetColorByIndex(colorIndex);
                return;
            }
            
            TMP_Text tmp = GetComponent<TMP_Text>();

            if (tmp)
            {
                tmp.color = _colorPalette.GetColorByIndex(colorIndex);
                return;
            }
            
            Camera camera = GetComponent<Camera>();

            if (camera)
            {
                camera.backgroundColor = _colorPalette.GetColorByIndex(colorIndex);
                return;
            }
        }
    }
}