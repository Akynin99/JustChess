using System;
using UnityEngine;

namespace JustChess
{
    public class SquareVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }
    }
}