using System.Linq;
using System.Threading;
using Boards;
using Boards.BoardCells;
using Cysharp.Threading.Tasks;
using Signals;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace GamePlay
{
    public class GamePlayController : MonoBehaviour, IInitializable
    {
        private ReactiveCollection<RegisteredUnit> _unitPacks = new();

        [Inject] private readonly SignalBus _signalBus;
        [Inject] private Board _board;

        private bool _enableBattle = true;
        private CancellationTokenSource _unitEndTurnToken;

        public void Initialize()
        {
            _unitPacks.ObserveAdd().Subscribe(_ => OnAddUnitPack(_)).AddTo(this);

            _signalBus.Subscribe<FulfillBoardNewUnitPackSignal>(ModifyUnitList);
            //_signalBus.Subscribe<SkeletonHitSignal>(OnSkeletonHitted);
            _signalBus.Subscribe<NextTurnSignal>(SetNextTurn);
        }

        private void OnAddUnitPack(CollectionAddEvent<RegisteredUnit> collectionAddEvent)
        {
            SortForInitiative();

            collectionAddEvent.Value.UnitPack.OnDead.Subscribe(_ => UnBindPack(collectionAddEvent.Value))
                .AddTo(collectionAddEvent.Value.CompositeDisposable);
        }

        private void SortForInitiative()
        {
            for (var i = 1; i < _unitPacks.Count; i++)
            {
                var current = _unitPacks[i];
                var j = i - 1;

                while (j >= 0 && _unitPacks[j].UnitPack.UnitModel.Initiative < current.UnitPack.UnitModel.Initiative)
                {
                    _unitPacks[j + 1] = _unitPacks[j];
                    j--;
                }

                _unitPacks[j + 1] = current;
            }
        }

        private void UnBindPack(RegisteredUnit unit)
        {
            unit.CompositeDisposable.Dispose();
            unit.CompositeDisposable.Clear();

            _unitPacks.Remove(unit);
        }

        private void ModifyUnitList(FulfillBoardNewUnitPackSignal signal)
        {
            _unitPacks.Add(new RegisteredUnit
                { UnitPack = signal.UnitPack, CompositeDisposable = new CompositeDisposable() });
        }

        /*private void OnSkeletonHitted(SkeletonHitSignal value)
        {
            var skeleton = value.Skeleton;

            var cell = MatrixUtils<BoardCell>.Find(_board.BoardCells,
                cell => cell.AssignedUnitPack != null && cell.AssignedUnitPack.UnitModel == skeleton);
        }*/

        private void SetNextTurn()
        {
            _unitEndTurnToken.Cancel();
        }

        private void Start()
        {
            LetTheBattleBegin().Forget();
        }

        private async UniTaskVoid LetTheBattleBegin()
        {
            while (_enableBattle)
            {
                for(var i = 0;i<_unitPacks.Count;i++) 
                {
                    _unitEndTurnToken = new CancellationTokenSource();

                    var remainingPacks = _unitPacks.Where(unitPack => unitPack != _unitPacks[i]).ToList();
                    remainingPacks.ForEach(unitsPack => unitsPack.UnitPack.TakeSide(_unitPacks[i].UnitPack.ArmySide));

                    await _unitPacks[i].UnitPack.MakeTurn(_unitEndTurnToken);
                }
            }
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<NextTurnSignal>(SetNextTurn);
            _signalBus.Unsubscribe<FulfillBoardNewUnitPackSignal>(ModifyUnitList);
        }
    }
}