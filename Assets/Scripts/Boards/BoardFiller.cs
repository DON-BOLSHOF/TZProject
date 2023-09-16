using System;
using System.Collections.Generic;
using Boards.BoardCells;
using Signals;
using Units;
using UnityEngine;
using Zenject;

namespace Boards
{
    public class BoardFiller : MonoBehaviour
    {
        [SerializeField] private UnitPack _unitPackPrefab;
        [SerializeField] private List<PositionedUnitPack> _unitsPacks;

        [Inject] private UnitPackFactory _factory;
        [Inject] private SignalBus _signalBus;

        public void FullFillBoard(List<List<BoardCell>> matrix)
        {
            foreach (var pack in _unitsPacks)
            {
                var unitPackInstance = _factory.Create(pack.UnitModelPrefab, pack.ArmySide,pack.IsInversed);

                matrix[pack.LocalGridPosition.y][pack.LocalGridPosition.x].AssignUnitPack(unitPackInstance);

                _signalBus.Fire(new FulfillBoardNewUnitPackSignal
                    { UnitPack = unitPackInstance });
            }
        }
    }

    [Serializable]
    public class PositionedUnitPack
    {
        [field: SerializeField] public UnitModel UnitModelPrefab { get; private set; }
        [field: SerializeField] public ArmySide ArmySide { get; private set; }
        [field: SerializeField] public Vector2Int LocalGridPosition { get; private set; }

        [field: SerializeField] public bool IsInversed;
    }
}