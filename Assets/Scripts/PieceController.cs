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
        private Square[][] _squares;
        private List<Piece> _pieces;
        private ChessboardVisual _chessboardVisual;
        private PieceVisualController _pieceVisualController;

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
            _squares = new Square[ranksCount][];
            _pieces = new List<Piece>();
            int nextPieceIndex = 0;

            for (int i = 0; i < _squares.Length; i++)
            {
                _squares[i] = new Square[filesCount];
            }

            foreach (var position in initialPosition.Positions)
            {
                if (position.X >= filesCount || position.Y >= ranksCount) continue;

                ChessVector2 forwardDir = new ChessVector2(0, position.Color == PieceColor.White ? 1 : -1);

                Piece piece = new Piece(position.Type, position.Color, nextPieceIndex, forwardDir);

                nextPieceIndex++;
                
                _pieces.Add(piece);
                _squares[position.Y][position.X].Piece = piece;
            }
            
            OnPositionChanged?.Invoke();
        }

        public Square[][] GetPositions()
        {
            return _squares;
        }
        
        public Piece GetPieceOnSquare(ChessVector2 pos)
        {
            if (!IsPosExists(pos)) return null;

            return _squares[pos.Y][pos.X].Piece;
        }

        public List<ChessVector2> GetAvailableMoves(Piece movingPiece)
        {
            if (movingPiece == null)
            {
                Debug.LogError("Null movingPiece");
                return null;
            }

            ChessVector2 piecePos = new ChessVector2();
            bool pieceFound = false;

            for (var i = 0; i < _squares.Length; i++)
            {
                var rank = _squares[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    var square = rank[j];
                    if (square.Piece != movingPiece) continue;
                    pieceFound = true;
                    piecePos = new ChessVector2(j, i);
                }
            }

            if (!pieceFound)
            {
                Debug.LogError("Can't find piece position on the board!");
                return null;
            }

            List<ChessVector2> availableMoves = new List<ChessVector2>();

            var rule = _rulesSet.GetRuleForPieceType(movingPiece.Type);
            
            // en passant

            if (rule.OnlyMoveLikePawn)
            {
                var checkPos = piecePos + movingPiece.Forward;
                
                if (!IsPosExists(checkPos)) return availableMoves;
                
                var square = _squares[checkPos.Y][checkPos.X];

                if (square.Piece == null)
                {
                    availableMoves.Add(checkPos);

                    if (movingPiece.MovesCount == 0)
                    {
                        checkPos += movingPiece.Forward;
                        
                        if (!IsPosExists(checkPos)) return availableMoves;
                        
                        square = _squares[checkPos.Y][checkPos.X];

                        if (square.Piece == null)
                        {
                            availableMoves.Add(checkPos);
                        }
                    }
                }
                else
                {
                    return availableMoves;
                }
                
                return availableMoves;
            }

            if (rule.OnlyPatternL)
            {
                ChessVector2 checkPos;
                
                checkPos = piecePos + movingPiece.Forward * 2 + movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Forward * 2 - movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Forward + movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Forward - movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Back + movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Back - movingPiece.Right * 2;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Back * 2 + movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                checkPos = piecePos + movingPiece.Back * 2 - movingPiece.Right;
                if (IsPosExistsAndAvailable(checkPos, movingPiece.Color)) availableMoves.Add(checkPos);
                
                return availableMoves;
            }

            if (rule.CanMoveHorizontally)
            {
                var rank = _squares[piecePos.Y];

                for (int i = piecePos.X; i < rank.Length; i++)
                {
                    if(i == piecePos.X) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(piecePos.X - i) > rule.MaxDistance) break;
                    
                    var square = rank[i];
                    var pos = new ChessVector2(i, piecePos.Y);

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
                
                for (int i = piecePos.X; i >= 0; i--)
                {
                    if(i == piecePos.X) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(piecePos.X - i) > rule.MaxDistance) break;
                    
                    var square = rank[i];
                    var pos = new ChessVector2(i, piecePos.Y);

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
                for (int i = piecePos.Y; i < _squares.Length; i++)
                {
                    if(i == piecePos.Y) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(piecePos.Y - i) > rule.MaxDistance) break;
                    
                    var square = _squares[i][piecePos.X];
                    var pos = new ChessVector2(piecePos.X, i);

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
                
                for (int i = piecePos.Y; i >= 0; i--)
                {
                    if(i == piecePos.Y) continue;
                    if(rule.MaxDistance > 0 && Mathf.Abs(piecePos.Y - i) > rule.MaxDistance) break;
                    
                    var square = _squares[i][piecePos.X];
                    var pos = new ChessVector2(piecePos.X, i);

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
                    var checkPos = piecePos + direction;
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

        public bool IsMoveAvailable(Piece movingPiece, ChessVector2 targetPos)
        {
            if (movingPiece == null)
            {
                Debug.LogError("Null movingPiece");
            }
            
            var availableMoves = GetAvailableMoves(movingPiece);

            return availableMoves.Contains(targetPos);
        }

        public void MovePieceOnPos(Piece movingPiece, ChessVector2 targetPos)
        {
            if (movingPiece == null) return;

            ChessVector2 pieceOldPos = new ChessVector2();
            bool pieceFound = false;

            for (var i = 0; i < _squares.Length; i++)
            {
                var rank = _squares[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    if (rank[j].Piece != movingPiece) continue;
                    pieceFound = true;
                    pieceOldPos = new ChessVector2(j, i);
                    rank[j].Piece = null;
                }
            }

            if (!pieceFound) return;

            _squares[targetPos.Y][targetPos.X].Piece = movingPiece;
            movingPiece.MovesCount++;
            
            OnPositionChanged?.Invoke();
        }

        private bool IsPosExists(ChessVector2 pos)
        {
            if (pos.Y < 0 || pos.X < 0 || _squares.Length <= pos.Y || _squares[pos.Y].Length <= pos.X) return false;

            return true;
        }
        
        private Piece GetPieceOnPos(ChessVector2 pos)
        {
            if (_squares.Length <= pos.Y || _squares[pos.Y].Length <= pos.X) return null;

            return _squares[pos.Y][pos.X].Piece;
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