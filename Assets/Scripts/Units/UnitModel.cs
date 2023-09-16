using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        [SerializeField] private int _movementPoints;
        [SerializeField] private int _initiative;
        [SerializeField] private int _health;
        
        [SerializeField] protected Animator _animator;

        private static readonly int MovingKey = Animator.StringToHash("IsWalking");
        private static readonly int HitKey = Animator.StringToHash("Hit");
        private static readonly int DieKey = Animator.StringToHash("IsDead");

        public ReactiveCommand OnDead = new();

        public int Health => _health;
        public int MovementPoints => _movementPoints;
        public int Initiative => _initiative;

        public virtual void SetMoving(bool value)
        {
            GetComponent<Animator>().SetBool(MovingKey, value);
        }

        public virtual void SetHit(int hitValue)
        {
            _animator.SetTrigger(HitKey);
        }

        public virtual void SetDie()
        {
            _animator.SetBool(DieKey, true);
        }

        private void OnAnimatedDeath()
        {
            OnDead?.Execute();
        }
    }
}