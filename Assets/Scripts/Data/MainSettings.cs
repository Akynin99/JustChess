using UnityEngine;

namespace JustChess.Data
{
    [CreateAssetMenu(fileName = "MainSettings", menuName = "JustChess/MainSettings", order = 0)]
    public class MainSettings : ScriptableObject
    {
        [SerializeField] private ChessPosition defaultPosition;
        [SerializeField] private RulesSet defaultRulesSet;
        [SerializeField] private int boardRanksCount;
        [SerializeField] private int boardFilesCount;

        public ChessPosition DefaultPosition => defaultPosition;
        public RulesSet DefaultRulesSet => defaultRulesSet;
        public int RanksCount => boardRanksCount;
        public int FilesCount => boardFilesCount;
    }
}