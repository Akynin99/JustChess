using JustChess.Data;
using JustChess.UI;
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
        private GameplayUIController _gameplayUIController;
        

        [Inject]
        private void Construct(ChessboardVisual chessboardVisual, PieceController pieceController,
            PieceVisualController pieceVisualController, GameplayUIController gameplayUIController)
        {
            _chessboardVisual = chessboardVisual;
            _pieceController = pieceController;
            _pieceVisualController = pieceVisualController;
            _gameplayUIController = gameplayUIController;

            _pieceController.OnGameEnded += OnGameEnded;
        }

        private void Start()
        {
            _chessboardVisual.CreateBoard(_mainSettings.RanksCount, _mainSettings.FilesCount);
            
            _pieceController.CreateBoard(_mainSettings.RanksCount, _mainSettings.FilesCount, _mainSettings.DefaultPosition);
        }

        private void OnGameEnded(GameResult result)
        {
            _gameplayUIController.ShowGameResult(result);
        }
    }
}
