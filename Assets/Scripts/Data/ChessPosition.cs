using System;
using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "ChessPosition", menuName = "JustChess/ChessPosition", order = 0)]
    public class ChessPosition : ScriptableObject
    {
        [SerializeField] private PiecePosition[] positions;

        public PiecePosition[] Positions => positions;
        
        [Serializable]
        public struct PiecePosition
        {
            public PieceType Type;
            public PieceColor Color;
            public int X;
            public int Y;
        }
    }
}