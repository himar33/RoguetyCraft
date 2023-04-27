using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Generic
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
    public class Enemy : MonoBehaviour
    {
        public EnemyStats Stats { get; protected set; }

        [SerializeField] protected float _health;
        [SerializeField] protected float _attackDamage;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected float _moveSpeed;

        protected void Start()
        {
            EnemyStats _stats = new EnemyStats(_health, _attackDamage, _attackSpeed, _moveSpeed);
            Stats = _stats;
        }
    }
}
