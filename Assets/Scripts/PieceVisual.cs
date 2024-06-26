using System;
using UnityEngine;

namespace JustChess
{
    public class PieceVisual : MonoBehaviour
    {
        [HideInInspector] public bool Checked;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private int _index;
        private PieceType _pieceType;
        private PieceColor _pieceColor;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public int Index => _index;
        public PieceType PieceType => _pieceType;
        public PieceColor PieceColor => _pieceColor;
        
        public void SetIndex(int index)
        {
            _index = index;
        }

        public void SetSprite(Sprite sprite, PieceType pieceType, PieceColor pieceColor)
        {
            SetSprite(sprite, pieceType, pieceColor, Color.white);
        }
        
        public void SetSprite(Sprite sprite, PieceType pieceType, PieceColor pieceColor, Color color)
        {
            _pieceType = pieceType;
            _pieceColor = pieceColor;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
        }

        public void SetPos(Vector2 newPos)
        {
            _transform.position = newPos;
        }
        
        public void SetScale(float scale)
        {
            _transform.localScale = Vector3.one * scale;
        }
    }
}