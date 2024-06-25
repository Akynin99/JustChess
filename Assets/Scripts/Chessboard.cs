using System;
using JustChess.Data;
using JustChess.Utility;
using UnityEngine;
using Zenject;

namespace JustChess
{
    public class Chessboard : MonoBehaviour
    {
        [SerializeField] private SquareVisual squarePrefab;
        [SerializeField] private float squareSize = 1;
        [SerializeField] private bool firstColorIsDark = true;

        [Inject] private ColorPalette _colorPalette;
        
        private readonly int _ranksCount = 8;
        private readonly int _filesCount = 8;
        private Transform _squaresParent;
        private int _lastScreenWidth;
        private int _lastScreenHeight;

        private void Start()
        {
            CreateBoard();
        }

        private void Update()
        {
            if (_lastScreenHeight != Screen.height || _lastScreenWidth != Screen.width) RefreshBoardScale();
        }

        private void CreateBoard()
        {
            GameObject squaresParentObj = new GameObject();
            squaresParentObj.name = "Board";
            _squaresParent = squaresParentObj.transform;
            _squaresParent.parent = transform;
            
            float rankPos = - (_ranksCount / 2f - 0.5f) * squareSize;
            bool rankStartColor = firstColorIsDark;

           
            
            for (int i = 0; i < _ranksCount; i++)
            {
                float filePos = - (_filesCount / 2f - 0.5f) * squareSize;
                bool squareColor = rankStartColor;
                
                for (int j = 0; j < _filesCount; j++)
                {
                    Vector2 pos = new Vector2(filePos, rankPos);
                    
                    string fileText = null;
                    string rankText = null;

                    if (j == 0) rankText = (i + 1).ToString();
                    if (i == 0) fileText = Alphabet.GetLetterAtIndex(j);
                    
                    SpawnVisual(pos, squareColor, _squaresParent, rankText, fileText);
                    
                    squareColor = !squareColor;
                    
                    filePos += squareSize;
                }

                rankStartColor = !rankStartColor;
                rankPos += squareSize;
            }

            RefreshBoardScale();
        }

        private void SpawnVisual(Vector2 pos, bool darkColor, Transform parent, string rankText, string fileText)
        {
            SquareVisual newVisual = Instantiate(squarePrefab, parent);

            newVisual.transform.position = pos;

            int colorIdx = darkColor ? 1 : 4;
            int tmpColorIdx = darkColor ? 4 : 1;

            Color color = _colorPalette.GetColorByIndex(colorIdx);
            Color tmpColor = _colorPalette.GetColorByIndex(tmpColorIdx);

            newVisual.SetColor(color);
            newVisual.SetText(rankText, fileText, tmpColor);
        }

        private void RefreshBoardScale()
        {
            float cameraSize = Camera.main.orthographicSize;
            
            float boardSize = squareSize;
            if (_ranksCount >= _filesCount)
            {
                boardSize *= _ranksCount;
            }
            else
            {
                boardSize *= _filesCount;
            }
            
            float screenRatio = 1;
            
            if (Screen.width < Screen.height)
            {
                screenRatio = (float)Screen.width / Screen.height;
            }

            float boardScale = cameraSize / boardSize * 2f * screenRatio;

            _squaresParent.localScale = boardScale * Vector3.one;

            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }
    }
}