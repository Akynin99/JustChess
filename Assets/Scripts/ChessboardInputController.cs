using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace JustChess
{
    public class ChessboardInputController : MonoBehaviour
    {
        [SerializeField] private LayerMask chessboardLayerMask;
        
        private bool _isTouching;
        private Vector2 _lastMousePos;
        private Camera _camera;
        private ChessboardVisual _chessboardVisual;
        private PlayerSelectController _playerSelectController;

        [Inject]
        private void Construct(ChessboardVisual chessboardVisual, PlayerSelectController playerSelectController)
        {
            _chessboardVisual = chessboardVisual;
            _playerSelectController = playerSelectController;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            CheckTouchInput();
        }

        private void CheckTouchInput()
        {
            if (Input.GetMouseButton(0))
            {
                _lastMousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                
                if (!_isTouching)
                {
                    // start touching

                    Collider2D collider = Physics2D.OverlapPoint(_lastMousePos, chessboardLayerMask);

                    if (collider)
                    {
                        _playerSelectController.ClickOnBoard(_lastMousePos);
                    }
                   
                    _isTouching = true;
                }
                else
                {
                    // continue touching
                    
                    Collider2D collider = Physics2D.OverlapPoint(_lastMousePos, chessboardLayerMask);

                    if (collider)
                    {
                        
                    }
                }
            }
            else
            {
                if (_isTouching)
                {
                    // end touching

                    

                    
                    _isTouching = false;
                }
            }
        }
    }
}