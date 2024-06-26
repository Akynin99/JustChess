using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "PieceMovementRule", menuName = "JustChess/PieceMovementRule", order = 0)]
    public class PieceMovementRule : ScriptableObject
    {
        [SerializeField] private PieceType pieceType;
        [SerializeField] private bool canJump;
        [SerializeField] private bool canMoveHorizontally;
        [SerializeField] private bool canMoveVertically;
        [SerializeField] private bool canMoveDiagonally;
        [SerializeField] private int maxDistance;
        [SerializeField] private bool patternL;
        [SerializeField] private bool onlyAdvancing;
        [SerializeField] private bool specialFirstMove;
        [SerializeField] private int specialFirstMoveDistance;
        [SerializeField] private bool enPassant;
        [SerializeField] private bool canCastling;
        [SerializeField] private bool canPromotion;
        [SerializeField] private bool capturesOnlyLikePawn;
    }
}