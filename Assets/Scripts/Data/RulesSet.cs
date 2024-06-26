using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "RulesSet", menuName = "JustChess/RulesSet", order = 0)]
    public class RulesSet : ScriptableObject
    {
        [SerializeField] private PieceMovementRule[] rules;

        public PieceMovementRule GetRuleForPieceType(PieceType pieceType)
        {
            foreach (var rule in rules)
            {
                if (rule.PieceType == pieceType) return rule;
            }

            return null;
        }
    }
}