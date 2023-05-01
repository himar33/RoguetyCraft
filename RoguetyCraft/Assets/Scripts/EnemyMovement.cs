using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool CanSeeTarget => TargetCheck();
    public bool OnWall => WallCheck();
    public bool OnEdge => EdgeCheck();
    public bool IsGrounded => GroundCheck();
    public Vector2 Direction => _direction;

    [Header("Gound Collision")]
    [SerializeField, Range(0, 1)] private float _distance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Target Collision")]
    [SerializeField, Range(0, 20)] private float _targetDistance = 4f;
    [SerializeField] private LayerMask _targetLayer;

    private Rigidbody2D _rb;
    private Collider2D _col;
    private Vector2 _direction = Vector2.left;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponentInChildren<Collider2D>();
    }

    private void OnDrawGizmos()
    {
        _col = GetComponentInChildren<Collider2D>();
        Bounds _b = _col.bounds;

        Gizmos.color = Color.blue;
        //Edge check collision
        Gizmos.DrawLine(new(_b.center.x + (_b.size.x/2 * _direction.x), _b.min.y), new(_b.center.x + (_b.size.x/2 * _direction.x), _b.min.y - _distance));
        //Ground check collision
        Gizmos.DrawLine(new(_b.center.x, _b.min.y), new(_b.center.x, _b.min.y - _distance));
        //Wall check collision
        Gizmos.DrawLine(new(_b.center.x + (_b.size.x / 2 * _direction.x), _b.center.y), new(_b.center.x + ((_b.size.x/2 * _direction.x) + _distance * _direction.x), _b.center.y));

        Gizmos.color = Color.red;
        //Target check collision
        Gizmos.DrawLine(new(_b.center.x + (_b.size.x / 2 * _direction.x), _b.center.y), new(_b.center.x + ((_b.size.x / 2 * _direction.x) + _targetDistance * _direction.x), _b.center.y));
    }

    private bool TargetCheck()
    {
        return Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.center.y), _direction, _targetDistance, _targetLayer);
    }

    private bool WallCheck()
    {
        return Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.center.y), _direction, _distance, _groundLayer);
    }

    private bool EdgeCheck()
    {
        return Physics2D.Raycast(new(_col.bounds.center.x + (_col.bounds.size.x / 2 * _direction.x), _col.bounds.min.y), Vector2.down, _distance, _groundLayer);
    }

    private bool GroundCheck()
    {
        return Physics2D.Raycast(new(_col.bounds.center.x, _col.bounds.min.y), Vector2.down, _distance, _groundLayer);
    }

    public void ChangeDirection()
    {
        _direction *= -1;
    }
    public void SetVelocity(float velocity) { if (_rb != null) _rb.velocity = new Vector2(velocity, _rb.velocity.y); }
    public Vector2 GetVelocity() { return _rb.velocity; }
}
