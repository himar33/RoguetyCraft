using MyBox;
using RoguetyCraft.Enemy.Generic;
using RoguetyCraft.Enemy.Movement;
using RoguetyCraft.Enemy.States;
using RoguetyCraft.Generic.Interfaces;
using RoguetyCraft.Generic.Utility;
using UnityEngine;

namespace RoguetyCraft.Enemy.Controller
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public EnemyMovement EMovement => _enemyMovement;
        public EnemyAnimator EAnimator => _enemyAnimator;
        public EnemyStateMachine EStateMachine { get; private set; }
        public float Health { get => _health; set => _health = value; }

        [Separator("Stats")]
        [SerializeField] private float _health = 10f;
        //[SerializeField] private float _attack = 2f;
        //[SerializeField] private float _attackSpeed = 1f;

        [Separator("Hit Settings")]
        public float HitTime = 0.5f;
        public Color HitColorMask = Color.red;
        [ReadOnly] public Color InitialColorMask;
        [ReadOnly] public float LastVelocity = 0f;

        private EnemyMovement _enemyMovement;
        private EnemyAnimator _enemyAnimator;

        private void Awake()
        {
            Utilities.GetComponent(gameObject, out _enemyMovement);
            Utilities.GetComponent(gameObject, out _enemyAnimator);

            InitialColorMask = _enemyAnimator.Sprite.color;
        }

        private void Start()
        {
            EStateMachine = new EnemyStateMachine();

            EStateMachine.Add(new EnemyIdle(this));
            EStateMachine.Add(new EnemyPatrol(this));
            EStateMachine.Add(new EnemyChase(this));
            EStateMachine.Add(new EnemyAttack(this));
            EStateMachine.Add(new EnemyHit(this));
            EStateMachine.Add(new EnemyDead(this));

            EStateMachine.Set(EnemyStates.PATROL);
        }

        private void Update()
        {
            EStateMachine.Update();
        }

        private void FixedUpdate()
        {
            EStateMachine.FixedUpdate();
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                EStateMachine.Set(EnemyStates.DEAD);
            }
            else EStateMachine.Set(EnemyStates.HIT);
        }

        private void OnParticleCollision(GameObject other)
        {
            TakeDamage(other.GetComponentInParent<IDamager>().AttackDamage);
        }
    }
}
