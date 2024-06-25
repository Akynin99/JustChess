using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "PieceSpriteSet", menuName = "JustChess/PieceSpriteSet", order = 0)]
    public class PieceSpriteSet : ScriptableObject
    {
        [SerializeField] private PieceSpriteList whiteList;
        [SerializeField] private PieceSpriteList blackList;

        public enum ChessPieceName
        {
            Pawn = 0,
            King = 1,
            Queen = 2,
            Rook = 3,
            Bishop = 4,
            Knight = 5,
        }

        public enum ChessPieceColor
        {
            White = 0,
            Black = 1,
        }
        
        [Serializable]
        private class PieceSpriteList
        {
            [SerializeField] public Sprite pawn;
            [SerializeField] public Sprite king;
            [SerializeField] public Sprite queen;
            [SerializeField] public Sprite rook;
            [SerializeField] public Sprite bishop;
            [SerializeField] public Sprite knight;

            public Sprite GetSprite(ChessPieceName pieceName)
            {
                switch (pieceName)
                {
                    case ChessPieceName.Pawn:
                        return pawn;
                    
                    case ChessPieceName.King:
                        return king;
                    
                    case ChessPieceName.Queen:
                        return queen;
                    
                    case ChessPieceName.Rook:
                        return rook;
                    
                    case ChessPieceName.Bishop:
                        return bishop;
                    
                    case ChessPieceName.Knight:
                        return knight;
                    
                    default:
                        return null;
                }
            }
        }

        public Sprite GetSprite(ChessPieceName pieceName, ChessPieceColor color)
        {
            switch (color)
            {
                case ChessPieceColor.White:
                    return whiteList.GetSprite(pieceName);
                
                case ChessPieceColor.Black:
                    return blackList.GetSprite(pieceName);
                
                default:
                    return null;
            }
        }
    }
}