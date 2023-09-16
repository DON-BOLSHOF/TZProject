using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Signals;
using UniRx;
using Units.Interfaces;
using UnityEngine;
using Zenject;

namespace Units
{
    public class UnitPack : MonoBehaviour
    {
        [SerializeField] private int _unitBaseCount;
        [SerializeField] private UnitPackView _unitPackView;

        public ArmySide ArmySide { get; private set; }
        public UnitModel UnitModel { get; private set; }
        public int CurrentMovementPoints { get; private set; }

        [Inject] private SignalBus _signalBus;

        private Destination _destination;

        private readonly ReactiveProperty<int> _packHealth = new();
        public ReactiveProperty<UnitPackState> PackState { get; private set; }
        public ReactiveCommand<Vector2> OnArrived { get; } = new();
        public readonly ReactiveProperty<int> UnitCurrentCount = new();
        public readonly ReactiveCommand OnDead = new();
        
        

        public void Initialize(UnitModel unitModel, ArmySide armySide)
        {
            UnitModel = Instantiate(unitModel, transform);

            ArmySide = armySide;

            PackState = new();
        }

        private void Start()
        {
            UnitCurrentCount.Value = _unitBaseCount;
            _packHealth.Value = UnitModel.Health * _unitBaseCount;

            _packHealth.Subscribe(ModifyCount).AddTo(this);
            UnitCurrentCount.Where(value => value <= 0).Subscribe(_ => Death()).AddTo(this);

            _unitPackView.Initialize(this);
            _signalBus.Subscribe<DestinationSignal>(OnDestinationSelected);

            UnitModel.OnDead.Subscribe(_ => Destroy(gameObject)); // Сначала все отписки с борда пройдут, потом модель проанимирует и го умрет.
        }

        public async UniTask MakeTurn(CancellationTokenSource endTurnSource)
        {
            CurrentMovementPoints = UnitModel.MovementPoints;
            PackState.Value = UnitPackState.ActiveUnit;

            while (!endTurnSource.Token.IsCancellationRequested)
            {
                _destination = null;

                var isCanceled = await UniTask
                    .WaitUntil(() => _destination != null, PlayerLoopTiming.Update, endTurnSource.Token)
                    .SuppressCancellationThrow();

                if (isCanceled) break;

                switch (_destination.DestinationType)
                {
                    case DestinationType.Walk:
                        if (_destination.Path.Count > 0)
                            await MoveTo(_destination.Path);
                        break;
                    case DestinationType.Attack:
                        if (UnitModel is IRangeAttackerUnit rangeAttackerUnit)
                        {
                            rangeAttackerUnit.SetRangeAttacking(_destination.Target);
                            _destination.Target.ModifyHealth(
                                rangeAttackerUnit.RangeAttackValue * UnitCurrentCount.Value - 1);
                        }

                        if (UnitModel is IMeleeAttackerUnit attackerUnit)
                        {
                            if (_destination.Path.Count > 0)
                                await MoveTo(_destination.Path);

                            attackerUnit.SetAttacking(_destination.Target);
                            _destination.Target.ModifyHealth(attackerUnit.AttackValue * UnitCurrentCount.Value * -1);
                            
                            endTurnSource.Cancel();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async UniTask MoveTo(List<Vector2> path)
        {
            UnitModel.SetMoving(true);

            foreach (var point in path)
            {
                CheckDirection(point);
                await transform.DOMove(point, 0.25f);
            }

            CurrentMovementPoints -= _destination.Path.Count;
            OnArrived?.Execute(_destination.Path[^1]);
            UnitModel.SetMoving(false);
        }

        private void CheckDirection(Vector2 point)
        {
            if (transform.localScale.x > 0 && point.x < transform.position.x
                || transform.localScale.x < 0 && point.x > transform.position.x)
                InverseDirection();
        }

        private void OnDestinationSelected(DestinationSignal signal)
        {
            if (PackState.Value != UnitPackState.ActiveUnit) return;

            _destination = signal.Destination;
        }

        private void ModifyHealth(int value)
        {
            _packHealth.Value += value;

            if (value < 0) UnitModel.SetHit(value);
        }

        public void InverseDirection()
        {
            var temp = transform.localScale;
            temp.x *= -1;

            transform.localScale = temp;
            _unitPackView.transform.localScale = temp;
        }

        private void ModifyCount(int newPackHealth)
        {
            UnitCurrentCount.Value = newPackHealth / UnitModel.Health;

            if (newPackHealth % UnitModel.Health != 0) UnitCurrentCount.Value++;
            if (UnitCurrentCount.Value < 0) UnitCurrentCount.Value = 0;
        }

        private void Death()
        {
            OnDead?.Execute();
            UnitModel.SetDie();
        }

        public void TakeSide(ArmySide packArmySide)
        {
            PackState.Value = packArmySide == ArmySide ? UnitPackState.Ally : UnitPackState.Enemy;
        }
    }

    public enum UnitPackState
    {
        Ally,
        ActiveUnit,
        Enemy
    }

    public enum ArmySide
    {
        FirstTeam,
        SecondTeam
    }
}