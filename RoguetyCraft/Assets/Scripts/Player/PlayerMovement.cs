using MyBox;
using RoguetyCraft.Items.Controller;
using RoguetyCraft.Player.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace RoguetyCraft.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public Vector3 PVelocity => _rb.velocity;
        public float PHorizontalRawInput { get; private set; }
        public float PHorizontalInput { get; private set; }
        public bool PJumpDown { get; private set; }
        public bool PJumpUp { get; private set; }
        public Collider2D PCollider { get; private set; }
        public Vector2 PDirection { get; private set; }

        public bool IsJumping { get; private set; }
        public bool IsRunning => (_colDown && _rb.velocity.x != .0f);
        public bool IsGrounded => _colDown;

        [Separator("Collision")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _collisionOffset = 0.1f;

        [Separator("Movement")]
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _acceleration = 80f;
        [SerializeField] private float _deAcceleration = 50f;

        [Separator("Jump")]
        [SerializeField] private float _jumpForce = 25f;
        [SerializeField] private float _coyoteTimeLimit = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField, Range(1, 10)] private float _jumpEndModifier = 1f;
        [SerializeField] private float _gravityModifier = 5f;
        [SerializeField] private float _maxFallingSpeed = -20f;

        [Separator("Events")]
        [SerializeField] private UnityEvent _onFlip;
        [SerializeField] private UnityEvent _onJump;
        [SerializeField] private UnityEvent _onLand;

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
            Gizmos.DrawWireCube(new(pBounds.center.x, pBounds.min.y), new(pBounds.size.x, _collisionOffset));
            Gizmos.DrawWireCube(new(pBounds.center.x, pBounds.max.y), new(pBounds.size.x, _collisionOffset));
            Gizmos.DrawWireCube(new(pBounds.min.x, pBounds.center.y), new(_collisionOffset, pBounds.size.y));
            Gizmos.DrawWireCube(new(pBounds.max.x, pBounds.center.y), new(_collisionOffset, pBounds.size.y));
        }

        private void GatherInput()
        {
            PJumpDown = Input.GetKeyDown(KeyCode.Space);
            PJumpUp = Input.GetKeyUp(KeyCode.Space);
            PHorizontalRawInput = Input.GetAxisRaw("Horizontal");
            PHorizontalInput = Input.GetAxis("Horizontal");

            Vector2 lastDirection = PDirection;

            if (PHorizontalRawInput != 0)
            {
                PDirection = (PHorizontalRawInput > 0) ? Vector2.right : Vector2.left;
            }

            if (PDirection != lastDirection && IsGrounded)
            {
                _onFlip.Invoke();
            }

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

            var groundCheck = Physics2D.BoxCast(new(_colCenter.x, _colMin.y), new(_colSize.x, _collisionOffset), 0, Vector2.down, _collisionOffset, _groundLayer);
            if (_colDown && !groundCheck) _timeGrounded = Time.time;
            else if (!_colDown && groundCheck)
            {
                _coyoteActive = true;
                _onLand.Invoke();
            }

            _colDown = Physics2D.BoxCast(new(_colCenter.x, _colMin.y), new(_colSize.x, _collisionOffset), 0, Vector2.down, _collisionOffset, _groundLayer);
            _colUp = Physics2D.BoxCast(new(_colCenter.x, _colMax.y), new(_colSize.x, _collisionOffset), 0, Vector2.up, _collisionOffset, _groundLayer);
            _colLeft = Physics2D.BoxCast(new(_colMin.x, _colCenter.y), new(_collisionOffset, _colSize.y), 0, Vector2.left, _collisionOffset, _groundLayer);
            _colRight = Physics2D.BoxCast(new(_colMax.x, _colCenter.y), new(_collisionOffset, _colSize.y), 0, Vector2.right, _collisionOffset, _groundLayer);
        }

        private void CalculateMovement()
        {
            if (PHorizontalRawInput != 0)
            {
                _currHorizontalSpeed += PHorizontalRawInput * _acceleration * Time.deltaTime;
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
                _onJump.Invoke();
                _currVerticalSpeed = _jumpForce;
                _pendingToJump = false;
            }
        }

        private void Move()
        {
            Vector2 currVelocity = new(_currHorizontalSpeed, _currVerticalSpeed);
            _rb.velocity = currVelocity;
        }
    }
}