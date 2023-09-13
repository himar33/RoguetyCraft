using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using RoguetyCraft.Enemies.Generic;

namespace RoguetyCraft.Enemies.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        #region Public Properties

        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        public float MoveSpeed => _moveSpeed;

        /// <summary>
        /// Gets the speed when chasing a target.
        /// </summary>
        public float ChaseSpeed => _chaseMoveSpeed;

        /// <summary>
        /// Gets the time to stop chasing.
        /// </summary>
        public float ChangeStopTime => _changeStopTime;

        /// <summary>
        /// Checks if the enemy can attack the target.
        /// </summary>
        public bool CanAttack => (GetTargetDistance() < _attackDistance);

        /// <summary>
        /// Checks if the enemy can see the target.
        /// </summary>
        public bool CanSeeTarget => TargetCheck();

        /// <summary>
        /// Checks if the enemy is on a wall.
        /// </summary>
        public bool OnWall => WallCheck();

        /// <summary>
        /// Checks if the enemy is near an edge.
        /// </summary>
        public bool OnEdge => EdgeCheck();

        /// <summary>
        /// Checks if the enemy is grounded.
        /// </summary>
        public bool IsGrounded => GroundCheck();

        /// <summary>
        /// Gets the current direction of the enemy.
        /// </summary>
        public Vector2 Direction => _direction;

        #endregion

        #region Serialized Fields

        [Separator("Movement")]
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _chaseMoveSpeed = 6f;
        [SerializeField] private float _changeStopTime = 2f;

        [Separator("Ground Collision")]
        [SerializeField, Range(0, 1)] private float _distance = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Separator("Target")]
        [SerializeField, Tag] private string _targetTag;
        [SerializeField, Range(0, 20)] private float _targetDistance = 4f;
        [SerializeField, Range(0, 5)] private float _attackDistance = 1f;
        [SerializeField] private LayerMask _targetLayer;

        #endregion

        #region Private Fields

        private Rigidbody2D _rb;
        private Collider2D _col;
        private Vector2 _direction = Vector2.left;
        private Transform _target;

        #endregion

        #region Unity Lifecycle

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Initializes required components.
        /// </summary>
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponentInChildren<Collider2D>();
            _target = GameObject.FindGameObjectWithTag(_targetTag).transform;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates movement and collision data.
        /// </summary>
        /// <param name="enemyData">Data to update movement parameters.</param>
        public void UpdateMovementData(Enemy enemyData)
        {
            _moveSpeed = enemyData.MoveSpeed;
            _chaseMoveSpeed = enemyData.ChaseMoveSpeed;
            _changeStopTime = enemyData.ChangeStopTime;
            _distance = enemyData.Distance;
            _groundLayer = enemyData.Groundlayer;
            _targetTag = enemyData.TargetTag;
            _targetDistance = enemyData.TargetDistance;
            _attackDistance = enemyData.AttackDistance;
            _targetLayer = enemyData.TargetLayer;
        }

        #endregion

        #region Gizmo Methods

        /// <summary>
        /// Draws gizmos for debugging and visualization.
        /// </summary>
        private void OnDrawGizmos()
        {
            _col = GetComponentInChildren<Collider2D>();
            Bounds _b = _col.bounds;

            // Draw ground and wall checks
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(_b.center.x + (_b.size.x / 2 * _direction.x), _b.min.y), new Vector2(_b.center.x + (_b.size.x / 2 * _direction.x), _b.min.y - _distance));
            Gizmos.DrawLine(new Vector2(_b.center.x, _b.min.y), new Vector2(_b.center.x, _b.min.y - _distance));
            Gizmos.DrawLine(new Vector2(_b.center.x + (_b.size.x / 2 * _direction.x), _b.center.y), new Vector2(_b.center.x + ((_b.size.x / 2 * _direction.x) + _distance * _direction.x), _b.center.y));

            // Draw target checks
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(_b.center.x + (_b.size.x / 2 * _direction.x), _b.center.y), new Vector2(_b.center.x + ((_b.size.x / 2 * _direction.x) + _targetDistance * _direction.x), _b.center.y));
        }

        #endregion

        #region Private Utility Methods

        /// <summary>
        /// Gets the distance to the target.
        /// </summary>
        public float GetTargetDistance()
        {
            return (_target.position - transform.position).magnitude;
        }

        /// <summary>
        /// Checks if the enemy can see the target without any obstacles.
        /// </summary>
        private bool TargetCheck()
        {
            RaycastHit2D targetHit = Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.center.y), _direction, _targetDistance, _targetLayer);

            if (!targetHit) return false;

            RaycastHit2D wallHit = Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.center.y), _direction, _targetDistance, _groundLayer);

            if (!wallHit) return true;

            float targetDist = targetHit.distance;
            float wallDist = wallHit.distance;
            return (targetDist < wallDist) ? true : false;
        }

        /// <summary>
        /// Checks if the enemy is touching a wall.
        /// </summary>
        private bool WallCheck()
        {
            return Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.center.y), _direction, _distance, _groundLayer);
        }

        /// <summary>
        /// Checks if the enemy is near an edge.
        /// </summary>
        private bool EdgeCheck()
        {
            return Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.min.y), Vector2.down, _distance, _groundLayer);
        }

        /// <summary>
        /// Checks if the enemy is grounded.
        /// </summary>
        private bool GroundCheck()
        {
            return Physics2D.Raycast(new(_col.bounds.center.x, _col.bounds.min.y), Vector2.down, _distance, _groundLayer);
        }

        /// <summary>
        /// Changes the direction of the enemy.
        /// </summary>
        public void ChangeDirection()
        {
            _direction *= -1;
        }

        #endregion

        #region Rigidbody Manipulation Methods

        /// <summary>
        /// Sets the velocity of the enemy.
        /// </summary>
        /// <param name="velocity">New velocity.</param>
        public void SetVelocity(float velocity)
        {
            if (_rb != null) _rb.velocity = new Vector2(velocity, _rb.velocity.y);
        }

        /// <summary>
        /// Gets the current velocity of the enemy.
        /// </summary>
        public Vector2 GetVelocity()
        {
            return _rb.velocity;
        }

        #endregion
    }
}