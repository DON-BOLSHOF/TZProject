using Units.Interfaces;
using UnityEngine;

namespace Units
{
    public class Knight : UnitModel, IMeleeAttackerUnit
    {
        [field: SerializeField] public int AttackValue { get; private set; }
        
        public void SetAttacking(UnitPack unitPack)
        {
            _animator.SetTrigger(IMeleeAttackerUnit.AttackKey);
        }
    }
}