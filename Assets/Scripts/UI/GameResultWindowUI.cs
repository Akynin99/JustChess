using TMPro;
using UnityEngine;

namespace JustChess.UI
{
    public class GameResultWindowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        public void ShowWinResult()
        {
            gameObject.SetActive(true);
            text.text = "You Win!";
        }
        
        public void ShowLooseResult()
        {
            gameObject.SetActive(true);
            text.text = "You Loose!";
        }
        
        public void ShowDrawResult()
        {
            gameObject.SetActive(true);
            text.text = "Draw!";
        }
    }
}