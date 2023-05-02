using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;
using RoguetyCraft.Generic.Animation;

namespace RoguetyCraft.Player.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : SpriteAnimator
    {
        private SpriteRenderer _sprite;
        private PlayerController _playerController;

        protected override void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _playerController = GetComponentInParent<PlayerController>();

            base.Awake();
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