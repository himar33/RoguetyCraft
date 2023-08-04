using RoguetyCraft.Enemies.Controller;
using RoguetyCraft.Generic.Animation;
using RoguetyCraft.Generic.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimator : SpriteAnimator
{
    public Animator Anim => _animator;
    public SpriteRenderer Sprite => _sprite;

    private SpriteRenderer _sprite;
    private EnemyController _enemy;

    protected override void Awake()
    {
        RoguetyUtilities.GetComponent(gameObject, out _sprite);
        RoguetyUtilities.GetComponent(gameObject, out _enemy);

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
