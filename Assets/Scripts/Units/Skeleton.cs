using Signals;
using Units.Interfaces;
using UnityEngine;
using Zenject;

namespace Units
{
    public class Skeleton : UnitModel, IMeleeAttackerUnit
    {
        [field: SerializeField] public int AttackValue { get; private set; }

        [Inject] private SignalBus _signalBus;

        public int SkeletonFaze { get; private set; } = 3;

        public void SetAttacking(UnitPack pack)
        {
            _animator.SetTrigger(IMeleeAttackerUnit.AttackKey);
        }

        public override void SetHit(int hitvalue)
        {
            base.SetHit(hitvalue);
            
            /*_signalBus.Fire(new SkeletonHitSignal{Skeleton = this, ValueToSpawn = Random.Range(1, hitvalue / Health)});
            
            SkeletonFaze--;*/
        }

        public void ModifyFieldsByPercent(int percent)
        {
            
        }
    }
}