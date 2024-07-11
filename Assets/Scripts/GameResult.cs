namespace JustChess
{
    public struct GameResult
    {
        public GameResultType Type;
        
        public enum GameResultType
        {
            Win = 0,
            Loose = 1,
            Draw = 2,
        }
    }
}