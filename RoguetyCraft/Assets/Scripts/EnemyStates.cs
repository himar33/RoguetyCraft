using RoguetyCraft.DesignPatterns.State;
using RoguetyCraft.Enemy.Controller;
using RoguetyCraft.Enemy.Generic;

namespace RoguetyCraft.Enemy.States
{
    public class EnemyState : State
    {
        // For convenience we will keep the ID for a State.
        // This ID represents the key
        public EnemyStates ID => _id;

        protected EnemyController _enemy = null;
        protected EnemyStates _id;

        public EnemyState(StateBehaviour sm, EnemyController enemy) : base(sm)
        {
            _enemy = enemy;
        }

        // A convenience constructor with just Player
        public EnemyState(EnemyController enemy) : base()
        {
            _enemy = enemy;
            _stateBehaviour = _enemy.StateMachine;
        }

        // The following are the normal methods from the State base class.
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
    public class EnemyStateMachine : StateBehaviour
    {
        public EnemyStateMachine() : base()
        {
        }

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
    }
    public class EnemyChase : EnemyState
    {
        public EnemyChase(EnemyController enemy) : base(enemy) { _id = EnemyStates.CHASE; }
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
