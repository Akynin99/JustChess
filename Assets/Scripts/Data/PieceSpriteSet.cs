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

        

        
        
        [Serializable]
        private class PieceSpriteList
        {
            [SerializeField] public Sprite pawn;
            [SerializeField] public Sprite king;
            [SerializeField] public Sprite queen;
            [SerializeField] public Sprite rook;
            [SerializeField] public Sprite bishop;
            [SerializeField] public Sprite knight;

            public Sprite GetSprite(PieceType pieceName)
            {
                switch (pieceName)
                {
                    case PieceType.Pawn:
                        return pawn;
                    
                    case PieceType.King:
                        return king;
                    
                    case PieceType.Queen:
                        return queen;
                    
                    case PieceType.Rook:
                        return rook;
                    
                    case PieceType.Bishop:
                        return bishop;
                    
                    case PieceType.Knight:
                        return knight;
                    
                    default:
                        return null;
                }
            }
        }

        public Sprite GetSprite(PieceType pieceName, PieceColor color)
        {
            switch (color)
            {
                case PieceColor.White:
                    return whiteList.GetSprite(pieceName);
                
                case PieceColor.Black:
                    return blackList.GetSprite(pieceName);
                
                default:
                    return null;
            }
        }
    }
}