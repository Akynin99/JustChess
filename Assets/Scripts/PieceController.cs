using System;
using System.Collections.Generic;
using JustChess.Data;
using UnityEngine;
using Zenject;

namespace JustChess
{
    public class PieceController : MonoBehaviour
    {
        public Action OnPositionChanged;

        [Inject] private MainSettings _mainSettings;
        
        private RulesSet _rulesSet;
        private Square[][] _currentBoard;
        private List<Piece> _pieces;
        private ChessboardVisual _chessboardVisual;
        private PieceVisualController _pieceVisualController;
        private PieceColor _currentTurn;

        private void Awake()
        {
            _rulesSet = _mainSettings.DefaultRulesSet;
        }

        [Inject]
        private void Construct(ChessboardVisual chessboardVisual, PieceVisualController pieceVisualController)
        {
            _chessboardVisual = chessboardVisual;
            _pieceVisualController = pieceVisualController;
        }

        public void CreateBoard(int ranksCount, int filesCount, ChessPosition initialPosition)
        {
            _currentBoard = new Square[ranksCount][];
            _pieces = new List<Piece>();
            int nextPieceIndex = 0;

            for (int i = 0; i < _currentBoard.Length; i++)
            {
                _currentBoard[i] = new Square[filesCount];
            }

            foreach (var position in initialPosition.Positions)
            {
                if (position.X >= filesCount || position.Y >= ranksCount) continue;

                ChessVector2 forwardDir = new ChessVector2(0, position.Color == PieceColor.White ? 1 : -1);

                Piece piece = new Piece(position.Type, position.Color, nextPieceIndex, forwardDir);

                nextPieceIndex++;
                
                _pieces.Add(piece);
                _currentBoard[position.Y][position.X].Piece = piece;
            }
            
            OnPositionChanged?.Invoke();
        }

        public Square[][] GetPositions()
        {
            return _currentBoard;
        }
        
        public Piece GetPieceOnSquare(ChessVector2 pos)
        {
            if (!IsPosExists(pos)) return null;

            return _currentBoard[pos.Y][pos.X].Piece;
        }

        private List<ChessVector2> GetAvailableMovesWithoutCheckingForCheckers(Square[][] board, Piece movingPiece, ChessVector2 movingPiecePos)
        {
            if (movingPiece == null)
            {
                Debug.LogError("Null movingPiece");
                return null;
            }

            List<ChessVector2> availableMoves = new List<ChessVector2>();

            var rule = _rulesSet.GetRuleForPieceType(movingPiece.Type);
            
            // en passant

            if (rule.OnlyMoveLikePawn)
            {
                var checkPos = movingPiecePos + movingPiece.Forward;

                if (IsPosExists(checkPos))
                {
                    var square = board[checkPos.Y][checkPos.X];

                    if (square.Piece == null)
                    {
                        availableMoves.Add(checkPos);

                        if (movingPiece.MovesCount == 0)
                        {
                            checkPos += movingPiece.Forward;
                        
                            if (!IsPosExists(checkPos)) return availableMoves;
                        
                            square = board[checkPos.Y][checkPos.X];

                            if (square.Piece == null)
                            {
                                availableMoves.Add(checkPos);
                            }
                        }
                    }
                }

                checkPos = movingPiecePos + movingPiece.RightForward;

                if (IsPosExists(checkPos))
                {
                    var square = board[checkPos.Y][checkPos.X];
                    
                    if (square.Piece != null && square.Piece.Color != movingPiece.Color) availableMoves.Add(checkPos);
                }
                
                checkPos = movingPiecePos + movingPiece.LeftForward;
                
                if (IsPosExists(checkPos))
                {
                    var square = board[checkPos.Y][checkPos.X];
                    
                    if (square.Piece != null && square.Piece.Color != movingPiece.Color) availableMoves.Add(checkPos);
                }
                
                return availableMoves;
            }

            if (rule.OnlyPatternL)
            {
                ChessVector2 checkPos;
                
                checkPos = movingPiecePos + movingPiece.Forward * 2 + movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Forward * 2 - movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Forward + movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Forward - movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Back + movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Back - movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Back * 2 + movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = movingPiecePos + movingPiece.Back * 2 - movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                return availableMoves;
            }

            if (rule.CanMoveHorizontally)
            {
                var rank = board[movingPiecePos.Y];

                for (int i = movingPiecePos.X; i < rank.Length; i++)
                {
                    if(i == movingPiecePos.X) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(movingPiecePos.X - i) > rule.MaxDistance) break;
                    
                    var square = rank[i];
                    var pos = new ChessVector2(i, movingPiecePos.Y);

                    if (square.Piece == null)
                    {
                        availableMoves.Add(pos);
                    }
                    else if (square.Piece.Color == movingPiece.Color)
                    {
                        break;
                    }
                    else
                    {
                        availableMoves.Add(pos);
                        break;
                    }
                }
                
                for (int i = movingPiecePos.X; i >= 0; i--)
                {
                    if(i == movingPiecePos.X) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(movingPiecePos.X - i) > rule.MaxDistance) break;
                    
                    var square = rank[i];
                    var pos = new ChessVector2(i, movingPiecePos.Y);

                    if (square.Piece == null)
                    {
                        availableMoves.Add(pos);
                    }
                    else if (square.Piece.Color == movingPiece.Color)
                    {
                        break;
                    }
                    else
                    {
                        availableMoves.Add(pos);
                        break;
                    }
                }
            }
            
            if (rule.CanMoveVertically)
            {
                for (int i = movingPiecePos.Y; i < board.Length; i++)
                {
                    if(i == movingPiecePos.Y) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(movingPiecePos.Y - i) > rule.MaxDistance) break;
                    
                    var square = board[i][movingPiecePos.X];
                    var pos = new ChessVector2(movingPiecePos.X, i);

                    if (square.Piece == null)
                    {
                        availableMoves.Add(pos);
                    }
                    else if (square.Piece.Color == movingPiece.Color)
                    {
                        break;
                    }
                    else
                    {
                        availableMoves.Add(pos);
                        break;
                    }
                }
                
                for (int i = movingPiecePos.Y; i >= 0; i--)
                {
                    if(i == movingPiecePos.Y) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(movingPiecePos.Y - i) > rule.MaxDistance) break;
                    
                    var square = board[i][movingPiecePos.X];
                    var pos = new ChessVector2(movingPiecePos.X, i);

                    if (square.Piece == null)
                    {
                        availableMoves.Add(pos);
                    }
                    else if (square.Piece.Color == movingPiece.Color)
                    {
                        break;
                    }
                    else
                    {
                        availableMoves.Add(pos);
                        break;
                    }
                }
            }

            if (rule.CanMoveDiagonally)
            {
                ChessVector2[] directions = new[]
                {
                    movingPiece.LeftForward, movingPiece.RightForward, movingPiece.LeftBack, movingPiece.RightBack
                };

                foreach (var direction in directions)
                {
                    var checkPos = movingPiecePos + direction;
                    int dist = 1;
                    while (!(rule.MaxDistance > 0 && dist > rule.MaxDistance) && IsPosExistsAndAvailable(checkPos, movingPiece.Color))
                    {
                        availableMoves.Add(checkPos);
                        
                        checkPos +=  direction;
                        dist++;
                    }
                }
                
            }

            return availableMoves;
        }

        private List<ChessVector2> RemoveCheckersMoves(Square[][] board, List<ChessVector2> moves, Piece movingPiece, ChessVector2 oldPiecePos)
        {
            List<ChessVector2> movesForRemoving = new List<ChessVector2>();
            
            foreach (var move in moves)
            {
                Square[][] newPosition = new Square[board.Length][];

                for (var i = 0; i < newPosition.Length; i++)
                {
                    newPosition[i] = new Square[board[i].Length];

                    for (int j = 0; j < newPosition[i].Length; j++)
                    {
                        newPosition[i][j] = board[i][j];
                    }
                }

                newPosition[oldPiecePos.Y][oldPiecePos.X].Piece = null;
                newPosition[move.Y][move.X].Piece = movingPiece;
                
                if(PositionHasCheck(newPosition, movingPiece.Color)) movesForRemoving.Add(move);
            }

            foreach (var moveForRemoving in movesForRemoving)
            {
                moves.Remove(moveForRemoving);
            }

            return moves;
        }

        private bool PositionHasCheck(Square[][] board, PieceColor kingColor)
        {
            ChessVector2 kingPosition = new ChessVector2();
            bool pieceFound = false;

            for (var i = 0; i < board.Length; i++)
            {
                var rank = board[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    var square = rank[j];
                    if (square.Piece == null || square.Piece.Color != kingColor || square.Piece.Type != PieceType.King) continue;
                    pieceFound = true;
                    kingPosition = new ChessVector2(j, i);
                    break;
                }
                
                if (pieceFound) break;
            }

            if (!pieceFound)
            {
                Debug.LogError("Can't find piece King!");
                return false;
            }

            for (var i = 0; i < board.Length; i++)
            {
                var rank = board[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    var square = rank[j];
                    if (square.Piece == null || square.Piece.Color == kingColor) continue;

                    ChessVector2 piecePos = new ChessVector2(j, i);

                    if (GetAvailableMovesWithoutCheckingForCheckers(board, square.Piece, piecePos)
                        .Contains(kingPosition)) return true;
                }
            }

            return false;
        }

        public List<ChessVector2> GetAvailableMoves(Piece movingPiece)
        {
            bool pieceFound = false;

            ChessVector2 movingPiecePos = new ChessVector2();

            for (var i = 0; i < _currentBoard.Length; i++)
            {
                var rank = _currentBoard[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    var square = rank[j];
                    if (square.Piece != movingPiece) continue;
                    pieceFound = true;
                    movingPiecePos = new ChessVector2(j, i);
                    break;
                }
                
                if (pieceFound) break;
            }

            if (!pieceFound)
            {
                Debug.LogError("Can't find piece position on the board!");
                return null;
            }
            
            var moves = GetAvailableMovesWithoutCheckingForCheckers(_currentBoard, movingPiece, movingPiecePos);

            moves = RemoveCheckersMoves(_currentBoard, moves, movingPiece, movingPiecePos);

            return moves;
        }

        public bool IsMoveAvailable(Piece movingPiece, ChessVector2 targetPos)
        {
            if (movingPiece == null)
            {
                Debug.LogError("Null movingPiece");
            }
            
            var availableMoves = GetAvailableMoves(movingPiece);

            return availableMoves.Contains(targetPos);
        }

        public void TryToMovePiece(Piece movingPiece, ChessVector2 targetPos)
        {
            if (movingPiece == null) return;
            
            if(_currentTurn != movingPiece.Color) return;

            ChessVector2 pieceOldPos = new ChessVector2();
            bool pieceFound = false;

            for (var i = 0; i < _currentBoard.Length; i++)
            {
                var rank = _currentBoard[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    if (rank[j].Piece != movingPiece) continue;
                    pieceFound = true;
                    pieceOldPos = new ChessVector2(j, i);
                    rank[j].Piece = null;
                }
            }

            if (!pieceFound) return;

            _currentBoard[targetPos.Y][targetPos.X].Piece = movingPiece;
            movingPiece.MovesCount++;

            int currentTurnIdx = (int)_currentTurn;
            var colorCount = Enum.GetNames(typeof(PieceColor)).Length;
            int nextTurn = (currentTurnIdx + 1) % colorCount;
            _currentTurn = (PieceColor)nextTurn;
            
            OnPositionChanged?.Invoke();
        }

        private bool IsPosExists(ChessVector2 pos)
        {
            if (pos.Y < 0 || pos.X < 0 || _currentBoard.Length <= pos.Y || _currentBoard[pos.Y].Length <= pos.X) return false;

            return true;
        }
        
        private Piece GetPieceOnPos(ChessVector2 pos)
        {
            if (_currentBoard.Length <= pos.Y || _currentBoard[pos.Y].Length <= pos.X) return null;

            return _currentBoard[pos.Y][pos.X].Piece;
        }

        private bool IsPosExistsAndAvailable(ChessVector2 pos, PieceColor movingPieceColor)
        {
            if (IsPosExists(pos))
            {
                var piece = GetPieceOnPos(pos);
                if (piece == null || piece.Color != movingPieceColor) return true;
            }

            return false;
        }
    }
}