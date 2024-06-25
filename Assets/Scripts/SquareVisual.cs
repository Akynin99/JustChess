using System;
using TMPro;
using UnityEngine;

namespace JustChess
{
    public class SquareVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text fileTmp;
        [SerializeField] private TMP_Text rankTmp;

        private void Awake()
        {
            if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }
        
        public void SetText(string rankText, string fileText, Color color)
        {
            if (rankText == null && fileText == null) canvas.gameObject.SetActive(false);
            
            canvas.gameObject.SetActive(true);

            fileTmp.text = fileText;
            rankTmp.text = rankText;
            fileTmp.color = color;
            rankTmp.color = color;
        }
    }
}