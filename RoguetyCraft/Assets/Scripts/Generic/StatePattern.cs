using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.DesignPatterns.State
{
    /// <summary>
    /// StateBehaviour class to manage state transitions and updates.
    /// </summary>
    public class StateBehaviour
    {
        #region Fields and Properties

        /// <summary>
        /// Gets the current active state.
        /// </summary>
        public State State => _currState;

        // Dictionary to hold the states mapped by an integer key.
        protected Dictionary<int, State> _states = new Dictionary<int, State>();

        // Reference to the current state.
        protected State _currState = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StateBehaviour() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new state to the collection.
        /// </summary>
        /// <param name="key">The key to associate with the state.</param>
        /// <param name="state">The state to add.</param>
        public void Add(int key, State state)
        {
            _states.Add(key, state);
        }

        /// <summary>
        /// Retrieves a state associated with the given key.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <returns>The associated state.</returns>
        public State GetState(int key)
        {
            return _states[key];
        }

        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <param name="state">The state to set as the current state.</param>
        public void Set(State state)
        {
            _currState?.OnExit();
            _currState = state;
            _currState?.OnEnter();
        }

        /// <summary>
        /// Updates the current state.
        /// </summary>
        public void Update()
        {
            _currState?.Update();
        }

        /// <summary>
        /// Performs fixed updates for the current state.
        /// </summary>
        public void FixedUpdate()
        {
            _currState?.FixedUpdate();
        }

        #endregion
    }

    /// <summary>
    /// Base class for a state that can be managed by StateBehaviour.
    /// </summary>
    public class State
    {
        #region Fields

        // Reference to the StateBehaviour instance managing this state.
        protected StateBehaviour _stateBehaviour;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with a reference to a StateBehaviour instance.
        /// </summary>
        /// <param name="stateBehaviour">The StateBehaviour instance.</param>
        public State(StateBehaviour stateBehaviour)
        {
            _stateBehaviour = stateBehaviour;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public State() { }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Method for state-specific logic that runs every frame.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Method for state-specific logic that runs every fixed frame.
        /// </summary>
        public virtual void FixedUpdate() { }

        /// <summary>
        /// Method called when entering the state.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Method called when exiting the state.
        /// </summary>
        public virtual void OnExit() { }

        #endregion
    }

}
