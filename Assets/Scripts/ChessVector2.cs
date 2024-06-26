namespace JustChess
{
    public struct ChessVector2
    {
        public int X;
        public int Y;

        public ChessVector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static ChessVector2 operator +(ChessVector2 a, ChessVector2 b) => new ChessVector2(a.X + b.X, a.Y + b.Y);
        public static ChessVector2 operator -(ChessVector2 a, ChessVector2 b) => new ChessVector2(a.X - b.X, a.Y - b.Y);
        public static ChessVector2 operator +(ChessVector2 a) => new ChessVector2(a.X, a.Y);
        public static ChessVector2 operator -(ChessVector2 a) => new ChessVector2(-a.X, -a.Y);
    }
}