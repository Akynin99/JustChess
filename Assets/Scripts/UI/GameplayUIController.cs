using UnityEngine;

namespace JustChess.UI
{
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private GameResultWindowUI gameResultWindowUI;

        public void ShowGameResult(GameResult result)
        {
            switch (result.Type)
            {
                case GameResult.GameResultType.Win:
                    gameResultWindowUI.ShowWinResult();
                    break;
                
                case GameResult.GameResultType.Loose:
                    gameResultWindowUI.ShowLooseResult();
                    break;
                
                case GameResult.GameResultType.Draw:
                    gameResultWindowUI.ShowDrawResult();
                    break;
            }
        }
    }
}