using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.Generic;
using RoguetyCraft.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Enemy.Controller
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyStats EStats { get; private set; }
        public EnemyMovement EMovement { get; private set; }
        public EnemyStateMachine EStateMachine { get; private set; }

        [Header("Enemy Stats")]
        [SerializeField] private float _health;
        [SerializeField] private float _attackDamage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _chaseMoveSpeed;

        private void Awake()
        {
            EMovement = GetComponent<EnemyMovement>();
        }

        private void Start()
        {
            EnemyStats _stats = new(_health, _attackDamage, _attackSpeed, _moveSpeed, _chaseMoveSpeed);
            EStats = _stats;

            EStateMachine = new EnemyStateMachine();

            EStateMachine.Add(new EnemyIdle(this));
            EStateMachine.Add(new EnemyPatrol(this));
            EStateMachine.Add(new EnemyChase(this));
            EStateMachine.Add(new EnemyAttack(this));
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
    }
}
