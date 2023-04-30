using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.DesignPatterns.State
{
    public class StateBehaviour
    {
        public State State => _currState;

        protected Dictionary<int, State> _states = new Dictionary<int, State>();
        protected State _currState = null;
        public StateBehaviour() { }
        public void Add(int key, State state)
        {
            _states.Add(key, state);
        }
        public State GetState(int key)
        {
            return _states[key];
        }
        public void Set(State state)
        {
            _currState?.OnExit();
            _currState = state;
            _currState?.OnEnter();
        }
        public void Update()
        {
            _currState?.Update();
        }
        public void FixedUpdate()
        {
            _currState?.FixedUpdate();
        }
    }

    public class State
    {
        protected StateBehaviour _stateBehaviour;
        public State(StateBehaviour stateBehaviour)
        {
            _stateBehaviour = stateBehaviour;
        }
        public State() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}
