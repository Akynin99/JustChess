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
        [SerializeField] private SpriteRenderer availableMoveMark;

        private Color _defaultColor;

        private void Awake()
        {
            if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
            
            availableMoveMark.enabled = false;
        }

        public void SetDefaultColor(Color color)
        {
            spriteRenderer.color = color;
            _defaultColor = color;
        }

        public void EnableHighlight(Color color)
        {
            spriteRenderer.color = color;
        }

        public void DisableHighlight()
        {
            spriteRenderer.color = _defaultColor;
        }

        public void SetAvailableMoveMarkActive(bool value)
        {
            availableMoveMark.enabled = value;
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