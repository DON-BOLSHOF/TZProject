using System.Collections.Generic;
using Boards.BoardCells;
using Signals;
using UniRx;
using Units;
using UnityEngine;
using Utils;
using Zenject;

namespace Boards
{
    public class BoardMouseDetector // Пофикси и внедри
    {
        private List<List<BoardCell>> _boardCells;
        private Grid _grid;
        private SignalBus _signalBus;

        private BoardCell _previousDetectedCell;

        public ReactiveCommand<BoardCell> OnPickedCellDetected = new();
        public ReactiveCommand<BoardCell> OnClickedCellDetected = new();

        public BoardMouseDetector(List<List<BoardCell>> cells, Grid grid,SignalBus signalBus)
        {
            _boardCells = cells;
            _signalBus = signalBus;
            _grid = grid;
            
            _signalBus.Subscribe<PickSignal>(DetectPickedCell);
            _signalBus.Subscribe<CellClickedSignal>(DetectClickedCell);
        }
        
        private void DetectPickedCell(PickSignal signal)
        {
            var boardCell = GetCellByGlobalPosition(signal.PickPosition);
            if (boardCell == null) return;

            if (_previousDetectedCell != null) OnPickedCellDetected?.Execute(boardCell);
        }

        private BoardCell GetCellByGlobalPosition(Vector2 Position)
        {
            var nearbyPosition = _grid.WorldToCell(Position);

            var position = (Vector2)_grid.GetCellCenterWorld(nearbyPosition);

            var boardCell = MatrixUtils<BoardCell>.Find(_boardCells, pos => pos.Position.Equals(position));

            return boardCell;
        }

        private void DetectClickedCell(CellClickedSignal signal)
        {
            var boardCellTo = GetCellByGlobalPosition(signal.CellClickedPosition);
            
            if(boardCellTo == null) return;

            OnClickedCellDetected?.Execute(boardCellTo);
        }

        ~BoardMouseDetector()
        {
            _signalBus.Unsubscribe<PickSignal>(DetectPickedCell);
            _signalBus.Unsubscribe<CellClickedSignal>(DetectClickedCell);
        }
    }
}