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
                Color color = _colorPalette.GetColorByIndex(colorIndex);
                color.a = image.color.a;
                image.color = color;
                return;
            }
            
            TMP_Text tmp = GetComponent<TMP_Text>();

            if (tmp)
            {
                Color color = _colorPalette.GetColorByIndex(colorIndex);
                color.a = tmp.color.a;
                tmp.color = color;
                return;
            }
            
            Camera camera = GetComponent<Camera>();

            if (camera)
            {
                Color color = _colorPalette.GetColorByIndex(colorIndex);
                color.a = camera.backgroundColor.a;
                camera.backgroundColor = color;
                return;
            }
        }
    }
}