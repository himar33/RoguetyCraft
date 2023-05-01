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
        public float ChaseMoveSpeed;

        public EnemyStats(float _health, float _attackDamage, float _attackSpeed, float _moveSpeed, float _chaseMoveSpeed)
        {
            Health = _health;
            AttackDamage = _attackDamage;
            AttackSpeed = _attackSpeed;
            MoveSpeed = _moveSpeed;
            ChaseMoveSpeed = _chaseMoveSpeed;
        }
    }
    public enum EnemyStates
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        DEAD
    }
}
