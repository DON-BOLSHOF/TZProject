using System;
using UniRx;
using Units;
using UnityEngine;

namespace Boards.BoardCells
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private BoardCellView _boardCellView;

        private BoardCellState _boardCellState = new();

        private UnitPack _assignedUnitPack;

        public Vector2 Position { get; private set; }
        public UnitPack AssignedUnitPack => _assignedUnitPack;
        public BoardCellStateHandler BoardCellStateHandler { get; private set; }

        public void DynamicInitialize(Vector2 position)
        {
            Position = position;

            BoardCellStateHandler = new BoardCellStateHandler(_boardCellState);

            _boardCellState.BoardCellVisualState.Subscribe(OnVisualStateChanged).AddTo(this);
        }

        private void OnVisualStateChanged(BoardCellVisualState visualState)
        {
            switch (visualState)
            {
                case BoardCellVisualState.None:
                    _boardCellView.SetCommonHighlight();
                    break;
                case BoardCellVisualState.MouseVisited:
                    _boardCellView.SetPickHighlight();
                    break;
                case BoardCellVisualState.ActiveUnitCell:
                    _boardCellView.SetActiveUnitHighlight();
                    break;
                case BoardCellVisualState.PotentialAttack:
                    _boardCellView.SetEnemyHighlight();
                    break;
                case BoardCellVisualState.PotentialMove:
                    _boardCellView.SetPotentialMoveHighlight();
                    break;
                case BoardCellVisualState.AllyCell:
                    _boardCellView.SetAllyHighlight();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visualState), visualState, null);
            }
        }

        public void AssignUnitPack(UnitPack pack)
        {
            _assignedUnitPack = pack;

            _assignedUnitPack.transform.parent = this.transform;

            _assignedUnitPack.transform.localPosition = Vector3.zero;
        }

        public void UnpinUnitPack()
        {
            _assignedUnitPack = null;
        }

        public void PaintSelf()
        {
            if (AssignedUnitPack != null)
            {
                switch (AssignedUnitPack.PackState.Value)
                {
                    case UnitPackState.Ally:
                        BoardCellStateHandler.SetAllyHighlight();
                        break;
                    case UnitPackState.ActiveUnit:
                        BoardCellStateHandler.SetActiveUnitCell();
                        break;
                    case UnitPackState.Enemy:
                        BoardCellStateHandler.SetEnemyHighlight();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                BoardCellStateHandler.SetPotentialMove();
            }
        }
    }
}