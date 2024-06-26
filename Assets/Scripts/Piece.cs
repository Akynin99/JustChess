using Unity.VisualScripting;

namespace JustChess
{
    public class Piece
    {
        public PieceType Type;
        public PieceColor Color;
        public int Index;
        public int MovesCount;
        public ChessVector2 Forward;
        public ChessVector2 Back;
        public ChessVector2 Right;
        public ChessVector2 Left;
        public ChessVector2 RightForward;
        public ChessVector2 LeftForward;

        public Piece(PieceType type, PieceColor color, int index, ChessVector2 forward)
        {
            Type = type;
            Color = color;
            Index = index;
            MovesCount = 0;
            Forward = forward;
            Back -= forward;

            int rightX = forward.Y;
            int rightY = -forward.X;
            
            Right = new ChessVector2(rightX, rightY);
            Left = -Right;
            RightForward = Forward + Right;
            LeftForward = Forward + Left;
        }
    }
}