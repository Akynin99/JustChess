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
        [SerializeField] private BoxCollider2D boxCollider;

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
        private Camera _camera;
        private float _scaledSquareSize;
        private SquareVisual[][] _squares;

        private void Awake()
        {
            _camera = Camera.main;
        }

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

            _squares = new SquareVisual[ranksCount][];
            for (int i = 0; i < ranksCount; i++)
            {
                _squares[i] = new SquareVisual[filesCount];
            }

            _lastScale = CalculateScale();
            _scaledSquareSize = squareSize * _lastScale;
            Vector2 boardSize = new Vector2(filesCount * _lastScale, ranksCount * _lastScale);
            boxCollider.size = boardSize;
            
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
                    
                    var newSquare = SpawnVisual(pos, squareColor, _squaresParent, rankText, fileText);
                    _squares[i][j] = newSquare;
                    
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

        private SquareVisual SpawnVisual(Vector2 pos, bool darkColor, Transform parent, string rankText, string fileText)
        {
            SquareVisual newVisual = Instantiate(squarePrefab, parent);

            newVisual.transform.position = pos;

            int colorIdx = darkColor ? 1 : 4;
            int tmpColorIdx = darkColor ? 4 : 1;

            Color color = _colorPalette.GetColorByIndex(colorIdx);
            Color tmpColor = _colorPalette.GetColorByIndex(tmpColorIdx);

            newVisual.SetDefaultColor(color);
            newVisual.SetText(rankText, fileText, tmpColor);

            return newVisual;
        }

        private void RefreshBoardScale()
        {
            _lastScale = CalculateScale();

            _scaledSquareSize = squareSize * _lastScale;
            _squaresParent.localScale = _lastScale * Vector3.one;

            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }

        private float CalculateScale()
        {
            float cameraSize = _camera.orthographicSize;
            
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

        public Vector2 ChessboardToGlobalPos(ChessVector2 pos)
        {
            return new Vector2(_squaresXPoses[pos.X], _squaresYPoses[pos.Y]);
        }

        public bool GlobalPosToChessboard(Vector2 globalPos, out ChessVector2 chessPos)
        {
            chessPos.X = -1;
            chessPos.Y = -1;

            if (globalPos.x < _squaresXPoses[0] && _squaresXPoses[0] - globalPos.x > _scaledSquareSize / 2f)
            {
                return false;
            }
            
            if (globalPos.x > _squaresXPoses[^1] && globalPos.x - _squaresXPoses[0] > _scaledSquareSize / 2f)
            {
                return false;
            }
            
            if (globalPos.y < _squaresYPoses[0] && _squaresYPoses[0] - globalPos.y > _scaledSquareSize / 2f)
            {
                return false;
            }
            
            if (globalPos.y > _squaresYPoses[^1] && globalPos.y - _squaresYPoses[0] > _scaledSquareSize / 2f)
            {
                return false;
            }

            float minDiff = float.MaxValue;

            for (int i = 0; i < _squaresXPoses.Length; i++)
            {
                float diff = Mathf.Abs(_squaresXPoses[i] - globalPos.x);
                
                if(diff >= minDiff) continue;

                minDiff = diff;
                chessPos.X = i;
            }
            
            minDiff = float.MaxValue;
            
            for (int i = 0; i < _squaresYPoses.Length; i++)
            {
                float diff = Mathf.Abs(_squaresYPoses[i] - globalPos.y);
                
                if(diff >= minDiff) continue;

                minDiff = diff;
                chessPos.Y = i;
            }

            return true;
        }

        public void DisableAllHighlights()
        {
            foreach (var rank in _squares)
            {
                foreach (var square in rank)
                {
                    square.DisableHighlight();
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void HighlightSquare(ChessVector2 pos)
        {
            if (pos.X < 0 || pos.Y < 0 || _squares.Length <= pos.Y || _squares[pos.Y].Length <= pos.X)
            {
                Debug.LogError("Wrong chessboard position!");
                return;
            }
            
            _squares[pos.Y][pos.X].EnableHighlight(_colorPalette.HighlightSquareColor);
        }

        public void EnableAvailableMoveMark(ChessVector2 pos)
        {
            _squares[pos.Y][pos.X].SetAvailableMoveMarkActive(true);
        }

        public void DisableAllAvailableMoveMarks()
        {
            foreach (var rank in _squares)
            {
                foreach (var square in rank)
                {
                    square.SetAvailableMoveMarkActive(false);
                }
            }
        }

        public float BoardScale => _lastScale;
    }
}