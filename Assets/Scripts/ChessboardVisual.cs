using System;
using System.Text;
using JustChess.Data;
using JustChess.Utility;
using UnityEngine;
using Zenject;

namespace JustChess
{
    public class ChessboardVisual : MonoBehaviour
    {
        [SerializeField] private SquareVisual squarePrefab;
        [SerializeField] private float squareSize = 1;
        [SerializeField] private bool firstColorIsDark = true;

        [Inject] private ColorPalette _colorPalette;
        
        private int _ranksCount;
        private int _filesCount;
        private Transform _squaresParent;
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private bool _boardCreated;
        private float[] _squaresXPoses;
        private float[] _squaresYPoses;
        private float _lastScale;

        private void Update()
        {
            if (!_boardCreated) return;
            
            if (_lastScreenHeight != Screen.height || _lastScreenWidth != Screen.width) RefreshBoardScale();
        }

        public void CreateBoard(int ranksCount, int filesCount)
        {
            _ranksCount = ranksCount;
            _filesCount = filesCount;
            
            _squaresXPoses = new float[filesCount];
            _squaresYPoses = new float[ranksCount];

            _lastScale = CalculateScale();
            
            GameObject squaresParentObj = new GameObject
            {
                name = "Board"
            };
            
            _squaresParent = squaresParentObj.transform;
            _squaresParent.parent = transform;
            
            float rankPos = - (_ranksCount / 2f - 0.5f) * squareSize;
            bool rankStartColor = firstColorIsDark;
            
            for (int i = 0; i < _ranksCount; i++)
            {
                float filePos = - (_filesCount / 2f - 0.5f) * squareSize;
                bool squareColor = rankStartColor;

                _squaresYPoses[i] = rankPos * _lastScale;
                
                for (int j = 0; j < _filesCount; j++)
                {
                    Vector2 pos = new Vector2(filePos, rankPos);
                    
                    string fileText = null;
                    string rankText = null;

                    if (j == 0) rankText = (i + 1).ToString();
                    if (i == 0) fileText = Alphabet.GetLetterAtIndex(j);
                    
                    SpawnVisual(pos, squareColor, _squaresParent, rankText, fileText);
                    
                    squareColor = !squareColor;
                    
                    _squaresXPoses[j] = filePos * _lastScale;
                    
                    filePos += squareSize;
                }

                rankStartColor = !rankStartColor;
                rankPos += squareSize;
            }

            RefreshBoardScale();
            
            _boardCreated = true;
        }

        private void DebugPoses()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var pos in _squaresXPoses)
            {
                stringBuilder.Append(" ").Append(pos);
            }
            
            Debug.Log(stringBuilder);

            stringBuilder.Clear();
                
            foreach (var pos in _squaresYPoses)
            {
                stringBuilder.Append(" ").Append(pos);
            }
            
            Debug.Log(stringBuilder);
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
            _squaresParent.localScale = CalculateScale() * Vector3.one;

            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }

        private float CalculateScale()
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

            return boardScale;
        }

        public Vector2 ChessboardToGlobalPos(int posX, int posY)
        {
            return new Vector2(_squaresXPoses[posX], _squaresYPoses[posY]);
        }

        public float BoardScale => _lastScale;
    }
}