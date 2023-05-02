using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Player.Animation
{
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    [RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip _idleClip;
        [SerializeField] private AnimationClip _runningClip;
        [SerializeField] private AnimationClip _jumpClip;
        [SerializeField] private AnimationClip _fallClip;

        private SpriteRenderer _sprite;
        private PlayerController _playerController;

        private Animator _animator;
        private AnimatorOverrideController _animatorOverride;
        private AnimationClipOverrides _animatorClips;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _playerController = GetComponentInParent<PlayerController>();

            _animator = GetComponent<Animator>();
            _animatorOverride = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverride;

            _animatorClips = new AnimationClipOverrides(_animatorOverride.overridesCount);
            _animatorOverride.GetOverrides(_animatorClips);

            _animatorClips["player_idle_anim"] = _idleClip;
            _animatorClips["player_run_anim"] = _runningClip;
            _animatorClips["player_jump_anim"] = _jumpClip;
            _animatorClips["player_fall_anim"] = _fallClip;
            _animatorOverride.ApplyOverrides(_animatorClips);
        }

        private void Update()
        {
            if (_playerController.PHorizontalRawInput != 0)
            {
                _sprite.flipX = (_playerController.PHorizontalRawInput > 0) ? false : true;
                _animator.speed = Mathf.Clamp(0.25f, 1f, Mathf.Abs(_playerController.PHorizontalInput));
            }
            else _animator.speed = 1;

            _animator.SetFloat("hSpeed", _playerController.PVelocity.x);
            _animator.SetFloat("vSpeed", _playerController.PVelocity.y);
            _animator.SetBool("isGrounded", _playerController.IsGrounded);
        }
    }
}