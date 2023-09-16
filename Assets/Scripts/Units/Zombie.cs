using Units.Interfaces;
using UnityEngine;

namespace Units
{
    public class Zombie : UnitModel, IMeleeAttackerUnit
    {
        [field: SerializeField] public int AttackValue { get; private set; }
        
        public void SetAttacking(UnitPack unitPack)
        {
            _animator.SetTrigger(IMeleeAttackerUnit.AttackKey);
            
        }

        private void NotifyPossess()
        {
            //Сигнал кинь
        }
    }
}