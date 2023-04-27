using RoguetyCraft.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Demo
{
    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer _sprite;
        private Animator _animator;
        private PlayerController _playerController;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _playerController = GetComponentInParent<PlayerController>();
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