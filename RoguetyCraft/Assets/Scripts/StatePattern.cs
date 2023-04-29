using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.DesignPatterns.State
{
    public class StateBehaviour
    {
        public State State => _state;
        protected State _state = null;
        public StateBehaviour(State state)
        {
            Set(state);
        }
        public void Set(State state)
        {
            _state?.OnExit();

            _state = state;

            _state.SetBehaviour(this);
            _state.OnEnter();
        }
        public void Update()
        {
            _state.Handle();
        }
    }

    public abstract class State
    {
        protected StateBehaviour _stateBehaviour;
        public void SetBehaviour(StateBehaviour stateBehaviour)
        {
            _stateBehaviour = stateBehaviour;
        }
        public abstract void Handle();
        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
