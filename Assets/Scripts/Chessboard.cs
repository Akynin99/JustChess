using System;
using JustChess.Data;
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
                    
                    SpawnVisual(pos, squareColor, _squaresParent);
                    
                    squareColor = !squareColor;
                    
                    filePos += squareSize;
                }

                rankStartColor = !rankStartColor;
                rankPos += squareSize;
            }

            RefreshBoardScale();
        }

        private void SpawnVisual(Vector2 pos, bool darkColor, Transform parent)
        {
            SquareVisual newVisual = Instantiate(squarePrefab, parent);

            newVisual.transform.position = pos;

            int colorIdx = darkColor ? 1 : 4;

            Color color = _colorPalette.GetColorByIndex(colorIdx);

            newVisual.SetColor(color);
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