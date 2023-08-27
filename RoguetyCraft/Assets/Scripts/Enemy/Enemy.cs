using UnityEngine;
using MyBox;
using RoguetyCraft.Generic.Animation;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace RoguetyCraft.Enemies.Generic
{
    [CreateAssetMenu(fileName = "enemy", menuName = "RoguetyCraft/Enemy")]
    public class Enemy : ScriptableObject
    {
        public float Health => _health;
        public float HitTime => _hitTime;
        public Color HitColorMask => _hitColorMask;
        public float MoveSpeed => _moveSpeed;
        public float ChaseMoveSpeed => _chaseMoveSpeed;
        public float ChangeStopTime => _changeStopTime;
        public float Distance => _distance;
        public LayerMask Groundlayer => _groundlayer;
        public string TargetTag => _targetTag;
        public float TargetDistance => _targetDistance;
        public float AttackDistance => _attackDistance;
        public LayerMask TargetLayer => _targetLayer;
        public AnimatorController AnimatorController => _animatorController;
        public List<AnimationClipVisual> Clips => _clips;

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

        [SerializeField, ReadOnly] private string  m_animatorController;

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

                    AnimationClipVisual clipVisual = new(stateName, clip);
                    _clips.Add(clipVisual);
                }
            }
        }
    }
}
