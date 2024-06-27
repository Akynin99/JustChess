using UnityEngine;
using Zenject;

namespace JustChess
{
    public class PlayerSelectController : MonoBehaviour
    {
        private PieceController _pieceController;
        private ChessboardVisual _chessboardVisual;
        private Piece _selectedPiece;
        

        private PieceColor _playerColor;
        
        private const bool PlayBothColors = true;
        
        [Inject]
        private void Construct(PieceController pieceController, ChessboardVisual chessboardVisual)
        {
            _pieceController = pieceController;
            _chessboardVisual = chessboardVisual;
        }
        
        public void ClickOnBoard(Vector2 pos)
        {
            if (!_chessboardVisual.GlobalPosToChessboard(pos, out var chessPos)) return;

            var piece = _pieceController.GetPieceOnSquare(chessPos);

            if (piece == null)
            {
                if(_selectedPiece != null)
                {
                    TryToMovePiece(_selectedPiece, chessPos);
                }
            }
            else
            {
                if(_selectedPiece != null && _selectedPiece.Color != piece.Color)
                {
                    TryToMovePiece(_selectedPiece, chessPos);
                }
                else if(piece.Color == _playerColor || PlayBothColors)
                {
                    SelectPiece(piece, chessPos);
                }
            }
        }

        private void SelectPiece(Piece piece, ChessVector2 chessPos)
        {
            if (_selectedPiece != null && _selectedPiece == piece)
            {
                _chessboardVisual.DisableAllHighlights();
                _chessboardVisual.DisableAllAvailableMoveMarks();
                _selectedPiece = null;
                return;
            }
            
            _chessboardVisual.DisableAllHighlights();
            _chessboardVisual.DisableAllAvailableMoveMarks();
            
            _selectedPiece = piece;
            
            _chessboardVisual.HighlightSquare(chessPos);

            var moves = _pieceController.GetAvailableMoves(piece);

            foreach (var move in moves)
            {
                _chessboardVisual.EnableAvailableMoveMark(move);
            }
        }

        private void TryToMovePiece(Piece piece, ChessVector2 chessPos)
        {
            _chessboardVisual.DisableAllHighlights();
            _chessboardVisual.DisableAllAvailableMoveMarks();
            _selectedPiece = null;
            
            if (!_pieceController.IsMoveAvailable(piece, chessPos)) return;

            _pieceController.TryToMovePiece(piece, chessPos);
        }
    }
}