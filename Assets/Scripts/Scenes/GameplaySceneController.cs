using JustChess.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace JustChess.Scenes
{
    public class GameplaySceneController : SceneController
    {
        [Inject] private MainSettings _mainSettings;

        private ChessboardVisual _chessboardVisual;
        private PieceController _pieceController;
        private PieceVisualController _pieceVisualController;

        [Inject]
        private void Construct(ChessboardVisual chessboardVisual, PieceController pieceController, PieceVisualController pieceVisualController)
        {
            _chessboardVisual = chessboardVisual;
            _pieceController = pieceController;
            _pieceVisualController = pieceVisualController;
        }

        private void Start()
        {
            _chessboardVisual.CreateBoard(_mainSettings.RanksCount, _mainSettings.FilesCount);
            
            _pieceController.CreateBoard(_mainSettings.RanksCount, _mainSettings.FilesCount, _mainSettings.DefaultPosition);
        }
    }
}
