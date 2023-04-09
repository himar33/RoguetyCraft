using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Vector3 Velocity;
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsGrounded => _isGrounded;

        [Header("Collision")]
        [SerializeField] private LayerMask _groundLayer;
        private bool _isGrounded;

        [Header("Movement")]
        [SerializeField] private float _acceleration = 90f;
        [SerializeField] private float _moveSpeed = 13f;
        [SerializeField] private float _deAcceleration = 60f;
        //[SerializeField] private float _apexBonus;

        [Header("Gravity")]
        [SerializeField] private float _gravityModifier;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;

        private void Update()
        {
            GatherInput();
            CalculateMovement();
            Move();
        }

        private void GatherInput()
        {
            IsJumping = Input.GetButtonDown("Jump");
            HorizontalInput = Input.GetAxisRaw("Horizontal");
        }

        private void CalculateMovement()
        {
            if (HorizontalInput != 0)
            {
                Velocity.x = Mathf.MoveTowards(Velocity.x, _moveSpeed * HorizontalInput, _acceleration * Time.deltaTime);
            }
            else
            {
                Velocity.x = Mathf.MoveTowards(Velocity.x, 0, _deAcceleration * Time.deltaTime);
            }
        }

        private void Move()
        {
            transform.Translate(Velocity * Time.deltaTime);
        }
    }
}
