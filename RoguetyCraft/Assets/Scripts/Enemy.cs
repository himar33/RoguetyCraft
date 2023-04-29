using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.States;

namespace RoguetyCraft.Enemy.Generic
{
    public struct EnemyStats
    {
        public float Health;
        public float AttackDamage;
        public float AttackSpeed;
        public float MoveSpeed;

        public EnemyStats(float _health, float _attackDamage, float _attackSpeed, float _moveSpeed)
        {
            Health = _health;
            AttackDamage = _attackDamage;
            AttackSpeed = _attackSpeed;
            MoveSpeed = _moveSpeed;
        }
    }
    public abstract class Enemy : MonoBehaviour
    {
        public EnemyStats Stats { get; protected set; }
        public StateBehaviour StateMachine { get; protected set; }

        [SerializeField] protected float _health;
        [SerializeField] protected float _attackDamage;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected float _moveSpeed;

        protected void Start()
        {
            EnemyStats _stats = new(_health, _attackDamage, _attackSpeed, _moveSpeed);
            Stats = _stats;

            StateMachine = new StateBehaviour(new EnemyIdle());
        }

        protected void Update()
        {
            StateMachine.Update();
        }
    }
}
