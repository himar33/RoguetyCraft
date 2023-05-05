using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;
using RoguetyCraft.Generic.Animation;
using System.Numerics;

namespace RoguetyCraft.Player.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : SpriteAnimator
    {
        private SpriteRenderer _sprite;
        private PlayerController _player;

        protected override void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _player = GetComponentInParent<PlayerController>();

            base.Awake();
        }

        private void Update()
        {
            if (_player.PHorizontalRawInput != 0)
            {
                _sprite.flipX = (_player.PHorizontalRawInput > 0) ? false : true;
            }
            else _animator.speed = 1;

            _animator.SetFloat("hSpeed", _player.PVelocity.x);
            _animator.SetFloat("vSpeed", _player.PVelocity.y);
            _animator.SetBool("isGrounded", _player.IsGrounded);
            //_animator.SetBool("isShooting", _player.PGun.IsShooting);
        }
    }
}