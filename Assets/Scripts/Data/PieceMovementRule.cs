using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "PieceMovementRule", menuName = "JustChess/PieceMovementRule", order = 0)]
    public class PieceMovementRule : ScriptableObject
    {
        [SerializeField] private PieceType pieceType;
        [SerializeField] private bool canMoveHorizontally;
        [SerializeField] private bool canMoveVertically;
        [SerializeField] private bool canMoveDiagonally;
        [SerializeField] private bool onlyMoveLikePawn;
        [SerializeField] private bool onlyPatternL;
        [SerializeField] private int maxDistance;
        [SerializeField] private bool enPassant;
        [SerializeField] private bool canCastling;
        [SerializeField] private bool canPromotion;
        
        
        public PieceType PieceType => pieceType;
        public bool CanMoveHorizontally => canMoveHorizontally;
        public bool CanMoveVertically => canMoveVertically;
        public bool CanMoveDiagonally => canMoveDiagonally;
        public bool OnlyMoveLikePawn => onlyMoveLikePawn;
        public bool OnlyPatternL => onlyPatternL;
        public int MaxDistance => maxDistance;
        public bool EnPassant => enPassant;
        public bool CanCastling => canCastling;
        public bool CanPromotion => canPromotion;
    }
}