using System;
using System.Collections.Generic;
using System.Linq;
using Boards.BoardCells;
using Signals;
using UniRx;
using Units;
using UnityEngine;
using Utils;
using Zenject;

namespace Boards
{
    [RequireComponent(typeof(BoardBuilder), typeof(BoardFiller))]
    public class Board : MonoBehaviour, IInitializable
    {
        [SerializeField] private Grid _grid;

        private BoardBuilder _boardBuilder;
        private BoardFiller _boardFiller;

        private List<List<BoardCell>> _boardCells = new();
        private BoardCell _previousDetectedCell;
        private List<RegisteredUnit> _currentPacks = new();

        [Inject] private SignalBus _signalBus;
        [Inject] private BoardNavigationSystem _boardNavigationSystem;

        public List<List<BoardCell>> BoardCells => _boardCells;

        public void Initialize()
        {
            _boardBuilder = GetComponent<BoardBuilder>();
            _boardFiller = GetComponent<BoardFiller>();

            _signalBus.Subscribe<PickSignal>(DetectPickedCell);
            _signalBus.Subscribe<CellClickedSignal>(DetectClickedCell);
            _signalBus.Subscribe<FulfillBoardNewUnitPackSignal>(AddToUnitList);

            _boardCells = _boardBuilder.GenerateBoard(15, 11);
            _boardFiller.FullFillBoard(_boardCells);
        }

        private void DetectPickedCell(PickSignal signal)
        {
            var boardCell = GetCellByGlobalPosition(signal.PickPosition);
            if (boardCell == null) return;

            if (_previousDetectedCell != null) _previousDetectedCell.BoardCellStateHandler.MouseExit();
            _previousDetectedCell = boardCell;

            boardCell.BoardCellStateHandler.MouseVisit();
        }

        public BoardCell GetCellByGlobalPosition(Vector2 Position)
        {
            var nearbyPosition = _grid.WorldToCell(Position);

            var position = (Vector2)_grid.GetCellCenterWorld(nearbyPosition);

            var boardCell = MatrixUtils<BoardCell>.Find(_boardCells, pos => pos.Position.Equals(position));

            return boardCell;
        }

        private void DetectClickedCell(CellClickedSignal signal)
        {
            var boardCellTo = GetCellByGlobalPosition(signal.CellClickedPosition);

            if (boardCellTo == null || boardCellTo.AssignedUnitPack != null &&
                boardCellTo.AssignedUnitPack.PackState.Value == UnitPackState.Ally) return;

            var registeredUnit = _currentPacks.First(_ => _.UnitPack.PackState.Value == UnitPackState.ActiveUnit);
            var boardCellFrom =
                MatrixUtils<BoardCell>.Find(_boardCells, cell => cell.AssignedUnitPack == registeredUnit.UnitPack);

            var path =
                _boardNavigationSystem.MakeDestination(boardCellFrom, boardCellTo,
                    registeredUnit.UnitPack.CurrentMovementPoints);

            var destination = DestinationType.Walk;
            UnitPack attackTarget = null;

            if (boardCellTo.AssignedUnitPack != null)
            {
                destination = DestinationType.Attack;
                attackTarget = boardCellTo.AssignedUnitPack;
            }

            _signalBus.Fire(new DestinationSignal
            {
                Destination = new Destination { DestinationType = destination, Path = path, Target = attackTarget }
            });
        }

        private void AddToUnitList(FulfillBoardNewUnitPackSignal signal)
        {
            var unit = signal.UnitPack;
            var compositeDisposable = new CompositeDisposable();


            _currentPacks.Add(new RegisteredUnit{ UnitPack = unit, CompositeDisposable = compositeDisposable });

            unit.PackState.Where(state => state == UnitPackState.ActiveUnit)
                .Subscribe(_ => ReloadBoardForNewUnitTurn(unit)).AddTo(compositeDisposable);
            unit.OnArrived.Subscribe(position => RelocateUnit(unit, position)).AddTo(compositeDisposable);
            unit.OnDead.Subscribe(_ => DestroyInUnitList(unit)).AddTo(compositeDisposable);
        }

        private void ReloadBoardForNewUnitTurn(UnitPack unitPack)
        {
            ClearBoard();
            SetVisualPotentialMovement(unitPack, unitPack.CurrentMovementPoints);
        }

        private void ClearBoard()
        {
            foreach (var boardCells in _boardCells)
            {
                foreach (var boardCell in boardCells)
                {
                    boardCell.BoardCellStateHandler.SetCommonHighlight();
                }
            }
        }

        private void SetVisualPotentialMovement(UnitPack unitPack, int searchDepth)
        {
            var unitCell = MatrixUtils<BoardCell>.Find(_boardCells, cell => cell.AssignedUnitPack == unitPack);
            if (unitCell == null) throw new ArgumentException("YOU HAVE BUG IN BOARD MANAGEMENT");

            var cellsToPaint = new Queue<BoardCell>();
            cellsToPaint.Enqueue(unitCell);

            for (int i = 0; i < searchDepth; i++)
            {
                PaintAroundCellsByQueue(cellsToPaint, cellsToPaint.Count);
            }
        }

        private void PaintAroundCellsByQueue(Queue<BoardCell> queue, int depth)
        {
            for (var i = 0; i < depth; i++)
            {
                var boardCell = queue.Dequeue();

                var arrayToCheck = new List<Vector3>();

                arrayToCheck.Add(new Vector3(boardCell.Position.x - 1, boardCell.Position.y));
                arrayToCheck.Add(new Vector3(boardCell.Position.x + 1, boardCell.Position.y));
                arrayToCheck.Add(new Vector3(boardCell.Position.x + 0.5f, boardCell.Position.y + 0.4875f));
                arrayToCheck.Add(new Vector3(boardCell.Position.x + 0.5f, boardCell.Position.y - 0.4875f));
                arrayToCheck.Add(new Vector3(boardCell.Position.x - 0.5f, boardCell.Position.y + 0.4875f));
                arrayToCheck.Add(new Vector3(boardCell.Position.x - 0.5f, boardCell.Position.y - 0.4875f));

                var temp = from position in arrayToCheck
                    select GetCellByGlobalPosition(position);

                foreach (var cell in temp)
                {
                    if (cell == null) continue;

                    queue.Enqueue(cell);
                    cell.PaintSelf();
                }
            }
        }

        private void RelocateUnit(UnitPack unitPack, Vector2 position)
        {
            var cellTo = GetCellByGlobalPosition(position);

            var cellFrom = MatrixUtils<BoardCell>.Find(_boardCells,
                _ => _.AssignedUnitPack != null && _.AssignedUnitPack.Equals(unitPack));

            cellFrom.UnpinUnitPack();

            cellTo.AssignUnitPack(unitPack);

            ReloadBoardForNewUnitTurn(unitPack);
        }

        private void DestroyInUnitList(UnitPack unit)
        {
            var registeredUnit = _currentPacks.Find(_ => _.UnitPack.Equals(unit));

            registeredUnit.CompositeDisposable.Clear();
            _currentPacks.Remove(registeredUnit);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PickSignal>(DetectPickedCell);
            _signalBus.Unsubscribe<CellClickedSignal>(DetectClickedCell);
            _signalBus.Unsubscribe<FulfillBoardNewUnitPackSignal>(AddToUnitList);
        }
    }

    public class RegisteredUnit
    {
        public UnitPack UnitPack;
        public CompositeDisposable CompositeDisposable;
    }
}