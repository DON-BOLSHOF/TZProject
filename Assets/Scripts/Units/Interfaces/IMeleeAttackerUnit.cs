using UnityEngine;

namespace Units.Interfaces
{
    public interface IMeleeAttackerUnit
    {
        public int AttackValue { get; }
        protected static readonly int AttackKey = Animator.StringToHash("Attack");
        public void SetAttacking(UnitPack unitPack);
    }
}