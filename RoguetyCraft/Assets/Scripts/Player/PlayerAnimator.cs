using RoguetyCraft.Player.Controller;
using UnityEngine;
using RoguetyCraft.Generic.Animation;
using RoguetyCraft.Generic.Utility;

namespace RoguetyCraft.Player.Animation
{
    public class PlayerAnimator : SpriteAnimator
    {
        private SpriteRenderer _sprite;

        protected override void Awake()
        {
            Utilities.GetComponent(gameObject, out _sprite);
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
    }
}