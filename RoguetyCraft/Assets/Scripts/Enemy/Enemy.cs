using UnityEngine;
using MyBox;
using RoguetyCraft.Generic.Animation;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace RoguetyCraft.Enemies.Generic
{
    /// <summary>
    /// Represents the data and behavior of an enemy entity in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "enemy", menuName = "RoguetyCraft/Enemy")]
    public class Enemy : ScriptableObject
    {
        #region Public Properties

        /// <summary>
        /// Gets the health of the enemy.
        /// </summary>
        public float Health => _health;

        /// <summary>
        /// Gets the hit time of the enemy.
        /// </summary>
        public float HitTime => _hitTime;

        /// <summary>
        /// Gets the color mask applied when the enemy is hit.
        /// </summary>
        public Color HitColorMask => _hitColorMask;

        /// <summary>
        /// Gets the enemy's move speed.
        /// </summary>
        public float MoveSpeed => _moveSpeed;

        /// <summary>
        /// Gets the enemy's move speed while chasing.
        /// </summary>
        public float ChaseMoveSpeed => _chaseMoveSpeed;

        /// <summary>
        /// Gets the time for which the enemy stops before changing direction.
        /// </summary>
        public float ChangeStopTime => _changeStopTime;

        /// <summary>
        /// Gets the distance threshold for ground detection.
        /// </summary>
        public float Distance => _distance;

        /// <summary>
        /// Gets the ground layer for collision detection.
        /// </summary>
        public LayerMask Groundlayer => _groundlayer;

        /// <summary>
        /// Gets the target tag for enemy targeting.
        /// </summary>
        public string TargetTag => _targetTag;

        /// <summary>
        /// Gets the distance within which a target becomes interesting for the enemy.
        /// </summary>
        public float TargetDistance => _targetDistance;

        /// <summary>
        /// Gets the distance within which the enemy initiates an attack.
        /// </summary>
        public float AttackDistance => _attackDistance;

        /// <summary>
        /// Gets the target layer for collision detection.
        /// </summary>
        public LayerMask TargetLayer => _targetLayer;

        /// <summary>
        /// Gets the animator controller for the enemy.
        /// </summary>
        public AnimatorController AnimatorController => _animatorController;

        /// <summary>
        /// Gets the list of animation clips for visual representation.
        /// </summary>
        public List<AnimationClipVisual> Clips => _clips;

        #endregion

        #region Serialized Fields

        [Separator("Enemy General")]
        [SerializeField] private float _health = 10f;
        [SerializeField] private float _hitTime = 0.5f;
        [SerializeField] private Color _hitColorMask = Color.red;

        [Space(20)]

        [Separator("Enemy Movement")]
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _chaseMoveSpeed = 6f;
        [SerializeField] private float _changeStopTime = 2f;

        [Space(20)]
        [SerializeField] private float _distance = 0.2f;
        [SerializeField] private LayerMask _groundlayer;

        [Space(20)]
        [Tag, SerializeField] private string _targetTag;
        [SerializeField] private float _targetDistance = 4f;
        [SerializeField] private float _attackDistance = 1f;
        [SerializeField] private LayerMask _targetLayer;

        [Space(20)]

        [Separator("Enemy Animator")]
        [SerializeField] private AnimatorController _animatorController;
        [SerializeField] private List<AnimationClipVisual> _clips;

        #endregion

        #region Private Fields

        [SerializeField, ReadOnly] private string m_animatorController;

        #endregion

        #region Unity Lifecycle Methods

        private void OnValidate()
        {
            if (_animatorController != null && m_animatorController != _animatorController.name)
            {
                Debug.Log($"{name} => m_animatorController:{m_animatorController} != _animatorController.name:{_animatorController.name}");
                m_animatorController = _animatorController.name;
                _clips.Clear();

                AnimatorControllerLayer layer = _animatorController.layers[0];
                for (int i = 0; i < layer.stateMachine.states.Length; i++)
                {
                    string stateName = layer.stateMachine.states[i].state.name;
                    AnimationClip clip = (AnimationClip)layer.stateMachine.states[i].state.motion;

                    AnimationClipVisual clipVisual = new AnimationClipVisual(stateName, clip);
                    _clips.Add(clipVisual);
                }
            }
        }

        #endregion
    }
}
