using System;
using System.Collections.Generic;
using JustChess.Data;
using UnityEngine;
using Zenject;

namespace JustChess
{
    public class PieceVisualController : MonoBehaviour
    {
        [SerializeField] private PieceVisual pieceVisualPrefab;
        [SerializeField] private int initialPoolSize = 64;

        [Inject] private PieceSpriteSet _pieceSpriteSet;
        
        private ChessboardVisual _chessboardVisual;
        private PieceController _pieceController;
        private List<PieceVisual> _pool = new List<PieceVisual>();
        private List<PieceVisual> _activePieces = new List<PieceVisual>();
        private Transform _poolParent;

        [Inject]
        private void Construct(ChessboardVisual chessboardVisual, PieceController pieceController)
        {
            _chessboardVisual = chessboardVisual;
            _pieceController = pieceController;
            _pieceController.OnPositionChanged += OnPositionChanged;
        }

        private void Awake()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            GameObject squaresParentObj = new GameObject
            {
                name = "Pool"
            };
            
            _poolParent = squaresParentObj.transform;
            _poolParent.parent = transform;
            
            for (int i = 0; i < initialPoolSize; i++)
            {
                PieceVisual newPiece = Instantiate(pieceVisualPrefab, _poolParent);
                newPiece.gameObject.SetActive(false);
                _pool.Add(newPiece);
            }
        }

        private void OnPositionChanged()
        {
            Square[][] positions = _pieceController.GetPositions();

            foreach (var pieceVisual in _activePieces)
            {
                pieceVisual.Checked = false;
            }

            for (var i = 0; i < positions.Length; i++)
            {
                var rank = positions[i];
                for (var j = 0; j < rank.Length; j++)
                {
                    var square = rank[j];
                    if (square.Piece == null) continue;

                    bool pieceVisualExists = false;

                    foreach (var activePiece in _activePieces)
                    {
                        if (activePiece.Index != square.Piece.Index) continue;

                        pieceVisualExists = true;
                        activePiece.Checked = true;
                        UpdatePieceVisual(activePiece, square.Piece, new ChessVector2(j, i));
                    }
                    
                    if (!pieceVisualExists) SpawnPieceVisual(square.Piece, new ChessVector2(j, i));
                }
            }

            List<PieceVisual> visualsForReturn = new List<PieceVisual>();

            foreach (var pieceVisual in _activePieces)
            {
                if (pieceVisual.Checked) continue;
                
                visualsForReturn.Add(pieceVisual);
            }

            foreach (var pieceVisual in visualsForReturn)
            {
                ReturnPieceVisualToPool(pieceVisual);
            }
        }

        private void SpawnPieceVisual(Piece piece, ChessVector2 pos)
        {
            PieceVisual visual = _pool[0];
            _pool.RemoveAt(0);
            
            _activePieces.Add(visual);
            visual.gameObject.SetActive(true);
            
            visual.SetIndex(piece.Index);
            visual.SetScale(_chessboardVisual.BoardScale);
            visual.Checked = true;
            
            ForceUpdateVisual(visual, piece, pos);
        }

        private void ForceUpdateVisual(PieceVisual visual, Piece piece, ChessVector2 pos)
        {
            visual.SetSprite(_pieceSpriteSet.GetSprite(piece.Type, piece.Color), piece.Type, piece.Color);
            
            visual.SetPos(_chessboardVisual.ChessboardToGlobalPos(pos));
        }

        private void UpdatePieceVisual(PieceVisual visual, Piece piece, ChessVector2 pos)
        {
            if (visual.PieceType != piece.Type || visual.PieceColor != piece.Color)
            {
                visual.SetSprite(_pieceSpriteSet.GetSprite(piece.Type, piece.Color), piece.Type, piece.Color);
            }
            
            visual.SetPos(_chessboardVisual.ChessboardToGlobalPos(pos));
        }

        private void ReturnPieceVisualToPool(PieceVisual visual)
        {
            visual.gameObject.SetActive(false);
            _activePieces.Remove(visual);
            _pool.Add(visual);
        }
    }
}