using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Vector3 pVelocity;
        public float pHorizontalInput { get; private set; }
        public float pVerticalInput { get; private set; }
        public Collider2D pCollider { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsGrounded => _isGrounded;

        [Header("Collision")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _collisionOffset = 0.01f;

        [Header("Movement")]
        [SerializeField] private float _acceleration = 90f;
        [SerializeField] private float _moveSpeed = 13f;
        [SerializeField] private float _deAcceleration = 60f;
        //[SerializeField] private float _apexBonus;

        [Header("Gravity")]
        [SerializeField] private float _gravityModifier;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;

        private bool _isGrounded;
        private bool _colUp, _colDown, _colLeft, _colRight;

        private void Awake()
        {
            pCollider = GetComponentInChildren<Collider2D>();
        }

        private void Update()
        {
            GatherInput();
            CheckCollisions();

            CalculateMovement();
            CalculateGravity();

            Move();
        }

        private void GatherInput()
        {
            IsJumping = Input.GetButtonDown("Jump");
            pHorizontalInput = Input.GetAxisRaw("Horizontal");
        }

        private void CheckCollisions()
        {
            Bounds pBounds = pCollider.bounds;

            _isGrounded = Physics2D.Raycast(pBounds.center, Vector2.down, pBounds.size.y / 2 + _collisionOffset, _groundLayer);

            _colDown = _isGrounded;
            _colUp = Physics2D.Raycast(pBounds.center, Vector2.up, pBounds.size.y / 2 + _collisionOffset, _groundLayer);
            _colLeft = Physics2D.Raycast(pBounds.center, Vector2.left, pBounds.size.x / 2 + _collisionOffset, _groundLayer);
            _colRight = Physics2D.Raycast(pBounds.center, Vector2.right, pBounds.size.x / 2 + _collisionOffset, _groundLayer);
        }

        private void CalculateMovement()
        {
            if (pHorizontalInput != 0)
            {
                pVelocity.x += pHorizontalInput * _acceleration * Time.deltaTime;
                pVelocity.x = Mathf.Clamp(pVelocity.x, -_moveSpeed, _moveSpeed);
            }
            else
            {
                pVelocity.x = Mathf.MoveTowards(pVelocity.x, 0, _deAcceleration * Time.deltaTime);
            }
        }

        private void CalculateGravity()
        {

        }

        private void Move()
        {
            transform.Translate(pVelocity * Time.deltaTime);
        }
    }
}
