using Units.Interfaces;
using UnityEngine;

namespace Units
{
    public class Archer: UnitModel, IMeleeAttackerUnit, IRangeAttackerUnit
    {
        [field: SerializeField] public int AttackValue { get; private set; }
        [field: SerializeField] public int RangeAttackValue { get; private set; }
        [SerializeField] private int _hexsToMegaShoot;
        
        public void SetAttacking(UnitPack unitPack)
        {
            _animator.SetTrigger(IMeleeAttackerUnit.AttackKey);

        }

        public void SetRangeAttacking(UnitPack unitPack)
        {
            _animator.SetTrigger(IRangeAttackerUnit.ShootKey);
        }
    }
}