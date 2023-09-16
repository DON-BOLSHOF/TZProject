using System;
using UnityEngine;

namespace Boards.BoardCells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BoardCellView : MonoBehaviour
    {
        private SpriteRenderer _view;

        [SerializeField] private Sprite _potentialMoveHighLight;
        [SerializeField] private Sprite _commonHighlight;
        [SerializeField] private Sprite _pickHighlight;
        [SerializeField] private Sprite _activeUnitHighlight;
        [SerializeField] private Sprite _allyHighlight;
        [SerializeField] private Sprite _enemyHighlight;
        
        private void Awake()
        {
            _view = GetComponent<SpriteRenderer>();
        }

        public void SetCommonHighlight()
        {
            _view.sprite = _commonHighlight;
        }

        public void SetPickHighlight()
        {
            _view.sprite = _pickHighlight;
        }

        public void SetActiveUnitHighlight()
        {
            _view.sprite = _activeUnitHighlight;
        }

        public void SetEnemyHighlight()
        {
            _view.sprite = _enemyHighlight;
        }

        public void SetPotentialMoveHighlight()
        {
            _view.sprite = _potentialMoveHighLight;
        }

        public void SetAllyHighlight()
        {
            _view.sprite = _allyHighlight;
        }
    }
}