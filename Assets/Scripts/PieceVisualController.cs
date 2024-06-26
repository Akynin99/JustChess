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

        private void Start()
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
                        UpdatePieceVisual(activePiece, square.Piece, j, i);
                    }
                    
                    if (!pieceVisualExists) SpawnPieceVisual(square.Piece, j, i);
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

        private void SpawnPieceVisual(Piece piece, int posX, int posY)
        {
            PieceVisual visual = _pool[0];
            _pool.RemoveAt(0);
            
            _activePieces.Add(visual);
            visual.gameObject.SetActive(true);
            
            visual.SetIndex(piece.Index);
            visual.SetScale(_chessboardVisual.BoardScale);
            visual.Checked = true;
            
            ForceUpdateVisual(visual, piece, posX, posY);
        }

        private void ForceUpdateVisual(PieceVisual visual, Piece piece, int posX, int posY)
        {
            visual.SetSprite(_pieceSpriteSet.GetSprite(piece.Type, piece.Color), piece.Type, piece.Color);
            
            visual.SetPos(_chessboardVisual.ChessboardToGlobalPos(posX, posY));
        }

        private void UpdatePieceVisual(PieceVisual visual, Piece piece, int posX, int posY)
        {
            if (visual.PieceType != piece.Type || visual.PieceColor != piece.Color)
            {
                visual.SetSprite(_pieceSpriteSet.GetSprite(piece.Type, piece.Color), piece.Type, piece.Color);
            }
            
            visual.SetPos(_chessboardVisual.ChessboardToGlobalPos(posX, posY));
        }

        private void ReturnPieceVisualToPool(PieceVisual visual)
        {
            visual.gameObject.SetActive(false);
            _activePieces.Remove(visual);
            _pool.Add(visual);
        }
    }
}