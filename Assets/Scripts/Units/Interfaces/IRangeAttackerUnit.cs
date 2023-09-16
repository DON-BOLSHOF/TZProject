using UnityEngine;

namespace Units.Interfaces
{
    public interface IRangeAttackerUnit
    {
        public int RangeAttackValue { get; }
        protected static readonly int ShootKey = Animator.StringToHash("Shoot");
        public void SetRangeAttacking(UnitPack unitPack);
    }
}