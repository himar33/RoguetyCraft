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

            StateMachine = new StateBehaviour(new EnemyPatrol(this));
        }

        protected void Update()
        {
            StateMachine.Update();
        }
    }
}
