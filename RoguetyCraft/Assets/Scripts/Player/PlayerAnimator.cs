using RoguetyCraft.Player.Controller;
using UnityEngine;
using RoguetyCraft.Generic.Animation;
using RoguetyCraft.Generic.Utility;
using UnityEditor.Animations;

namespace RoguetyCraft.Player.Animation
{
    public class PlayerAnimator : SpriteAnimator
    {
        private SpriteRenderer _sprite;
        private AnimatorController m_animatorController;

        protected override void Awake()
        {
            RoguetyUtilities.GetComponent(gameObject, out _sprite);
            base.Awake();
        }

        private void Update()
        {
            if (PlayerController.Instance.PlayerMovement.PHorizontalRawInput != 0)
            {
                _sprite.flipX = (PlayerController.Instance.PlayerMovement.PHorizontalRawInput > 0) ? false : true;
            }

            _animator.SetFloat("hSpeed", PlayerController.Instance.PlayerMovement.PVelocity.x);
            _animator.SetFloat("vSpeed", PlayerController.Instance.PlayerMovement.PVelocity.y);
            _animator.SetBool("isGrounded", PlayerController.Instance.PlayerMovement.IsGrounded);
            _animator.SetBool("isShooting", PlayerController.Instance.PlayerGun.IsShooting);
        }

        private void OnValidate()
        {
            if (m_animatorController != _animatorController)
            {
                m_animatorController = _animatorController;
                _animationClips.Clear();

                AnimatorControllerLayer layer = _animatorController.layers[0];
                for (int i = 0; i < layer.stateMachine.states.Length; i++)
                {
                    string stateName = layer.stateMachine.states[i].state.name;
                    AnimationClip clip = (AnimationClip)layer.stateMachine.states[i].state.motion;

                    AnimationClipVisual clipVisual = new(stateName, clip);
                    _animationClips.Add(clipVisual);
                }
            }
        }
    }
}