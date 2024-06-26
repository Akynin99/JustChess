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
        
        private Square[][] _squares;
        private List<Piece> _pieces;
        private ChessboardVisual _chessboardVisual;
        private PieceVisualController _pieceVisualController;
        
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
                
                Piece piece = new Piece
                {
                    Type = position.Type,
                    Color = position.Color,
                    Index = nextPieceIndex
                };

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
    }

    public struct Square
    {
        public Piece Piece;
    }

    public class Piece
    {
        public PieceType Type;
        public PieceColor Color;
        public int Index;
    }
}