using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "RulesSet", menuName = "JustChess/RulesSet", order = 0)]
    public class RulesSet : ScriptableObject
    {
        [SerializeField] private PieceMovementRule[] rules;
    }
}