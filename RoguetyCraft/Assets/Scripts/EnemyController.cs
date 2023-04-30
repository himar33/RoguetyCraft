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
        public EnemyStats Stats { get; private set; }
        public EnemyStateMachine StateMachine { get; private set; }

        [SerializeField] private float _health;
        [SerializeField] private float _attackDamage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _moveSpeed;

        private void Start()
        {
            EnemyStats _stats = new(_health, _attackDamage, _attackSpeed, _moveSpeed);
            Stats = _stats;

            StateMachine = new EnemyStateMachine();

            StateMachine.Add(new EnemyIdle(this));
            StateMachine.Add(new EnemyPatrol(this));
            StateMachine.Add(new EnemyChase(this));
            StateMachine.Add(new EnemyAttack(this));
            StateMachine.Add(new EnemyDead(this));

            StateMachine.Set(EnemyStates.IDLE);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }
    }
}
