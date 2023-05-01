using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.Controller;
using RoguetyCraft.Enemy.Generic;
using UnityEngine;

namespace RoguetyCraft.Enemy.States
{
    public class EnemyState : State
    {
        public EnemyStates ID => _id;

        protected EnemyController _enemy = null;
        protected EnemyStates _id;
        public EnemyState(StateBehaviour sm, EnemyController enemy) : base(sm)
        {
            _enemy = enemy;
        }
        public EnemyState(EnemyController enemy) : base()
        {
            _enemy = enemy;
            _stateBehaviour = _enemy.EStateMachine;
        }
        public override void OnEnter() { base.OnEnter(); }
        public override void OnExit() { base.OnExit(); }
        public override void Update() { base.Update(); }
        public override void FixedUpdate() { base.FixedUpdate(); }
    }
    public class EnemyStateMachine : StateBehaviour
    {
        public EnemyStateMachine() : base() { }
        public void Add(EnemyState state)
        {
            _states.Add((int)state.ID, state);
        }
        public EnemyState GetState(EnemyStates key)
        {
            return (EnemyState)GetState((int)key);
        }
        public void Set(EnemyStates stateKey)
        {
            State state = _states[(int)stateKey];
            if (state != null)
            {
                Set(state);
            }
        }
    }
    public class EnemyIdle : EnemyState
    {
        public EnemyIdle(EnemyController enemy) : base(enemy) { _id = EnemyStates.IDLE; }
    }
    public class EnemyPatrol : EnemyState
    {
        public EnemyPatrol(EnemyController enemy) : base(enemy) { _id = EnemyStates.PATROL; }
        public override void Update()
        {
            if (_enemy.EMovement.CanSeeTarget) _enemy.EStateMachine.Set(EnemyStates.CHASE);
            
            if ((!_enemy.EMovement.OnEdge || _enemy.EMovement.OnWall) && _enemy.EMovement.IsGrounded)
            {
                _enemy.EMovement.ChangeDirection();
            }
        }
        public override void FixedUpdate()
        {
            _enemy.EMovement.SetVelocity(_enemy.EMovement.Direction.x * _enemy.EStats.MoveSpeed);
        }
    }
    public class EnemyChase : EnemyState
    {
        public EnemyChase(EnemyController enemy) : base(enemy) { _id = EnemyStates.CHASE; }
        public override void Update()
        {
            if (!_enemy.EMovement.CanSeeTarget) _enemy.EStateMachine.Set(EnemyStates.PATROL);

            if ((!_enemy.EMovement.OnEdge || _enemy.EMovement.OnWall) && _enemy.EMovement.IsGrounded)
            {
                _enemy.EMovement.ChangeDirection();
            }
        }
        public override void FixedUpdate()
        {
            _enemy.EMovement.SetVelocity(_enemy.EMovement.Direction.x * _enemy.EStats.ChaseMoveSpeed);
        }
    }
    public class EnemyAttack : EnemyState
    {
        public EnemyAttack(EnemyController enemy) : base(enemy) { _id = EnemyStates.ATTACK; }
    }
    public class EnemyDead : EnemyState
    {
        public EnemyDead(EnemyController enemy) : base(enemy) { _id = EnemyStates.DEAD; }
    }
}
