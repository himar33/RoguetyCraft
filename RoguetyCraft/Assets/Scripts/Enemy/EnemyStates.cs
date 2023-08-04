using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemies.Controller;
using System.Collections;
using UnityEngine;

namespace RoguetyCraft.Enemies
{
    public enum EnemyStates
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        HIT,
        DEAD
    }
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
        public override void OnEnter()
        {
            _enemy.EMovement.SetVelocity(0f);
            _enemy.StartCoroutine(IdleStop());
        }
        public IEnumerator IdleStop()
        {
            yield return new WaitForSeconds(_enemy.EMovement.ChangeStopTime);
            _enemy.EStateMachine.Set((_enemy.EMovement.CanSeeTarget) ? EnemyStates.CHASE : EnemyStates.PATROL);
        }
    }
    public class EnemyPatrol : EnemyState
    {
        public EnemyPatrol(EnemyController enemy) : base(enemy) { _id = EnemyStates.PATROL; }
        public override void Update()
        {
            if ((!_enemy.EMovement.OnEdge || _enemy.EMovement.OnWall) && _enemy.EMovement.IsGrounded)
            {
                _enemy.EMovement.ChangeDirection();
                _enemy.EStateMachine.Set((_enemy.EMovement.CanSeeTarget) ? EnemyStates.CHASE : EnemyStates.IDLE);
            }

            if (_enemy.EMovement.CanSeeTarget) _enemy.EStateMachine.Set(EnemyStates.CHASE);
        }
        public override void FixedUpdate()
        {
            _enemy.EMovement.SetVelocity(_enemy.EMovement.Direction.x * _enemy.EMovement.MoveSpeed);
        }
        public override void OnExit()
        {
            _enemy.LastVelocity = _enemy.EMovement.GetVelocity().magnitude;
        }
    }
    public class EnemyChase : EnemyState
    {
        public EnemyChase(EnemyController enemy) : base(enemy) { _id = EnemyStates.CHASE; }
        public override void Update()
        {
            bool targetCheck = _enemy.EMovement.CanSeeTarget;

            if ((!_enemy.EMovement.OnEdge || _enemy.EMovement.OnWall) && _enemy.EMovement.IsGrounded)
            {
                _enemy.EMovement.ChangeDirection();
                _enemy.EStateMachine.Set((targetCheck) ? EnemyStates.CHASE : EnemyStates.IDLE);
            }

            if (!targetCheck) _enemy.EStateMachine.Set(EnemyStates.PATROL);
        }
        public override void FixedUpdate()
        {
            _enemy.EMovement.SetVelocity(_enemy.EMovement.Direction.x * _enemy.EMovement.ChaseSpeed);
        }
        public override void OnExit()
        {
            _enemy.LastVelocity = _enemy.EMovement.GetVelocity().magnitude;
        }
    }
    public class EnemyAttack : EnemyState
    {
        public EnemyAttack(EnemyController enemy) : base(enemy) { _id = EnemyStates.ATTACK; }
    }
    public class EnemyDead : EnemyState
    {
        public EnemyDead(EnemyController enemy) : base(enemy) { _id = EnemyStates.DEAD; }
        public override void OnEnter()
        {
            GameObject.Destroy(_enemy.gameObject);
        }
    }
    public class EnemyHit : EnemyState
    {
        public EnemyHit(EnemyController enemy) : base(enemy) { _id = EnemyStates.HIT; }
        public override void OnEnter()
        {
            _enemy.EMovement.SetVelocity(0f);
            _enemy.EAnimator.Anim.enabled = false;
            _enemy.EAnimator.Sprite.color = _enemy.HitColorMask;

            _enemy.StartCoroutine(HitPause());
        }
        public override void OnExit()
        {
            _enemy.EAnimator.Sprite.color = _enemy.InitialColorMask;
            _enemy.EAnimator.Anim.enabled = true;
            _enemy.EMovement.SetVelocity(_enemy.LastVelocity);
        }
        public IEnumerator HitPause()
        {
            yield return new WaitForSecondsRealtime(_enemy.HitTime);
            _enemy.EStateMachine.Set((_enemy.EMovement.CanSeeTarget) ? EnemyStates.CHASE : EnemyStates.IDLE);
        }
    }
}
