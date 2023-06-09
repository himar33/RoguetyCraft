using RoguetyCraft.Enemy.Controller;
using RoguetyCraft.Generic.Animation;
using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimator : SpriteAnimator
{
    private SpriteRenderer _sprite;
    private EnemyController _enemy;

    protected override void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _enemy = GetComponentInParent<EnemyController>();

        base.Awake();
    }

    private void Update()
    {
        if (_enemy.EMovement.GetVelocity().x != 0)
        {
            _sprite.flipX = (_enemy.EMovement.Direction.x > 0) ? false : true;
            _animator.speed = Mathf.Lerp(1f, 2f, _enemy.EMovement.GetVelocity().Abs().x / _enemy.EMovement.ChaseSpeed);
        }

        _animator.SetFloat("hSpeed", _enemy.EMovement.GetVelocity().x);
    }
}
