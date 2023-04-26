using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public Vector3 PVelocity => _rb.velocity;
        public float PHorizontalInput { get; private set; }
        public bool PJumpDown { get; private set; }
        public bool PJumpUp { get; private set; }
        public Collider2D PCollider { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsGrounded => _colDown;

        [Header("Collision")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _collisionOffset = 0.01f;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _acceleration = 80f;
        [SerializeField] private float _deAcceleration = 50f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 25f;
        [SerializeField] private float _coyoteTimeLimit = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndModifier = 0.1f;
        [SerializeField] private float _gravityModifier = 5f;
        [SerializeField] private float _maxFallingSpeed = -20f;

        private Rigidbody2D _rb;
        private float _currHorizontalSpeed, _currVerticalSpeed;

        private Vector3 _colSize, _colCenter, _colMin, _colMax;
        private bool _colUp, _colDown, _colLeft, _colRight;

        private float _timeGrounded;
        private float _lastJumpPressed;
        private bool _pendingToJump;
        private bool _endedJump = true;
        private bool _coyoteActive;
        private bool CanUseCoyote => _coyoteActive && !_colDown && _timeGrounded + _coyoteTimeLimit > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        private void Awake()
        {
            PCollider = GetComponentInChildren<Collider2D>();

            _rb = GetComponentInChildren<Rigidbody2D>();
            _rb.gravityScale = _gravityModifier;
        }

        private void Update()
        {
            GatherInput();
            CheckCollisions();

            CalculateMovement();
            CalculateJump();
        }

        private void FixedUpdate()
        {
            CheckMovesToDo();
            Move();
        }

        private void OnDrawGizmos()
        {
            Bounds pBounds = GetComponentInChildren<Collider2D>().bounds;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(pBounds.center.x, pBounds.min.y), new Vector2(pBounds.size.x, _collisionOffset));
            Gizmos.DrawWireCube(new Vector2(pBounds.center.x, pBounds.max.y), new Vector2(pBounds.size.x, _collisionOffset));
            Gizmos.DrawWireCube(new Vector2(pBounds.min.x, pBounds.center.y), new Vector2(_collisionOffset, pBounds.size.y));
            Gizmos.DrawWireCube(new Vector2(pBounds.max.x, pBounds.center.y), new Vector2(_collisionOffset, pBounds.size.y));
        }

        private void GatherInput()
        {
            PJumpDown = Input.GetKeyDown(KeyCode.Space);
            PJumpUp = Input.GetKeyUp(KeyCode.Space);
            PHorizontalInput = Input.GetAxisRaw("Horizontal");

            if (PJumpDown)
            {
                _lastJumpPressed = Time.time;
            }
        }

        private void CheckCollisions()
        {
            _colSize = PCollider.bounds.size;
            _colCenter = PCollider.bounds.center;
            _colMin = PCollider.bounds.min;
            _colMax = PCollider.bounds.max;

            var groundCheck = Physics2D.BoxCast(new Vector2(_colCenter.x, _colMin.y), new Vector2(_colSize.x, _collisionOffset), 0, Vector2.down, _collisionOffset, _groundLayer);
            if (_colDown && !groundCheck) _timeGrounded = Time.time;
            else if (!_colDown && groundCheck)
            {
                _coyoteActive = true;
            }

            _colDown = Physics2D.BoxCast(new Vector2(_colCenter.x, _colMin.y), new Vector2(_colSize.x, _collisionOffset), 0, Vector2.down, _collisionOffset, _groundLayer);
            _colUp = Physics2D.BoxCast(new Vector2(_colCenter.x, _colMax.y), new Vector2(_colSize.x, _collisionOffset), 0, Vector2.up, _collisionOffset, _groundLayer);
            _colLeft = Physics2D.BoxCast(new Vector2(_colMin.x, _colCenter.y), new Vector2(_collisionOffset, _colSize.y), 0, Vector2.left, _collisionOffset, _groundLayer);
            _colRight = Physics2D.BoxCast(new Vector2(_colMax.x, _colCenter.y), new Vector2(_collisionOffset, _colSize.y), 0, Vector2.right, _collisionOffset, _groundLayer);
        }

        private void CalculateMovement()
        {
            if (PHorizontalInput != 0)
            {
                _currHorizontalSpeed += PHorizontalInput * _acceleration * Time.deltaTime;
                _currHorizontalSpeed = Mathf.Clamp(_currHorizontalSpeed, -_moveSpeed, _moveSpeed);
            }
            else _currHorizontalSpeed = Mathf.MoveTowards(_currHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);

            if ((_currHorizontalSpeed < 0 && _colLeft) || (_currHorizontalSpeed > 0 && _colRight)) _currHorizontalSpeed = 0;

            var currGravity = (_endedJump && _currVerticalSpeed > 0) ? _gravityModifier * _jumpEndModifier : _gravityModifier;
            _rb.gravityScale = currGravity;

            _currVerticalSpeed = _rb.velocity.y;
            if (_currVerticalSpeed < _maxFallingSpeed) _currVerticalSpeed = _maxFallingSpeed;
        }

        private void CalculateJump()
        {
            if (PJumpDown && CanUseCoyote || HasBufferedJump)
            {
                _pendingToJump = true;

                _endedJump = false;
                _coyoteActive = false;
                _timeGrounded = float.MinValue;
                IsJumping = true;
            }
            else IsJumping = false;

            if (!_colDown && PJumpUp && !_endedJump && PVelocity.y > 0) _endedJump = true;

            if (_colUp)
            {
                if (_currVerticalSpeed > 0) _currVerticalSpeed = 0;
            }
        }

        private void CheckMovesToDo()
        {
            if (_pendingToJump)
            {
                _currVerticalSpeed = _jumpForce;
                _pendingToJump = false;
            }
        }

        private void Move()
        {
            Vector2 currVelocity = new Vector2(_currHorizontalSpeed, _currVerticalSpeed);
            _rb.velocity = currVelocity;
        }
    }
}
