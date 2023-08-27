using RoguetyCraft.Enemies.Movement;
using RoguetyCraft.Generic.Interfaces;
using RoguetyCraft.Generic.Utility;
using UnityEngine;
using UnityEditor;
using RoguetyCraft.Enemies.Generic;
using RoguetyCraft.Player.Movement;
using MyBox;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using RoguetyCraft.Generic.Animation;

namespace RoguetyCraft.Enemies.Controller
{
    [RequireLayer("Enemy")]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Enemy _enemyData;

        public EnemyMovement EMovement => _enemyMovement;
        public EnemyAnimator EAnimator => _enemyAnimator;
        public EnemyStateMachine EStateMachine { get; private set; }
        public float Health { get => _health; set => _health = value; }
        public float HitTime { get => _hitTime; set => _hitTime = value; }
        public Color HitColorMask { get => _hitColorMask; set => _hitColorMask = value; }
        public Color InitialColorMask { get => _initialColorMask; set => _initialColorMask = value; }
        public float LastVelocity { get => _lastVelocity; set => _lastVelocity = value; }

        private float _health;
        private float _hitTime;
        private Color _hitColorMask;
        private Color _initialColorMask;
        private float _lastVelocity = 0f;

        private EnemyMovement _enemyMovement;
        private EnemyAnimator _enemyAnimator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            RoguetyUtilities.GetComponent(gameObject, out _enemyMovement);
            RoguetyUtilities.GetComponent(gameObject, out _enemyAnimator);
            RoguetyUtilities.GetComponent(gameObject, out _spriteRenderer);

            _initialColorMask = _spriteRenderer.color;
        }

        private void Start()
        {
            EStateMachine = new EnemyStateMachine();

            EStateMachine.Add(new EnemyIdle(this));
            EStateMachine.Add(new EnemyPatrol(this));
            EStateMachine.Add(new EnemyChase(this));
            EStateMachine.Add(new EnemyAttack(this));
            EStateMachine.Add(new EnemyHit(this));
            EStateMachine.Add(new EnemyDead(this));

            EStateMachine.Set(EnemyStates.PATROL);
        }

        private void Update()
        {
            EStateMachine.Update();
        }

        private void FixedUpdate()
        {
            EStateMachine.FixedUpdate();
        }

        private void OnParticleCollision(GameObject other)
        {
            TakeDamage(other.GetComponentInParent<IDamager>().AttackDamage);
        }

        private void OnValidate()
        {
            if (_enemyData != null)
            {
                _health = _enemyData.Health;
                _hitTime = _enemyData.HitTime;
                _hitColorMask = _enemyData.HitColorMask;

                if (RoguetyUtilities.GetComponent(gameObject, out EnemyMovement movement))
                {
                    movement.UpdateMovementData(_enemyData);
                }
                if (RoguetyUtilities.GetComponent(gameObject, out EnemyAnimator animator))
                {
                    animator.AnimatorController = _enemyData.AnimatorController;

                    List<AnimationClipVisual> animationClipVisuals = new();
                    for (int i = 0; i < _enemyData.Clips.Count; i++)
                    {
                        AnimationClipVisual animationClipVisual = new(_enemyData.Clips[i].KeyName, _enemyData.Clips[i].ValueClip);
                        animationClipVisuals.Add(animationClipVisual);
                    }
                    animator.Animations = animationClipVisuals;

                    if (RoguetyUtilities.GetComponent(gameObject, out SpriteRenderer spriteR))
                    {
                        spriteR.sprite = RoguetyUtilities.GetSpritesFromClip(_enemyData.Clips[0].ValueClip)[0];
                        if (RoguetyUtilities.GetComponent(gameObject, out BoxCollider2D collider))
                        {
                            Vector2 S = spriteR.bounds.size;
                            collider.size = S;
                        }
                    }
                }
            }
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                EStateMachine.Set(EnemyStates.DEAD);
            }
            else EStateMachine.Set(EnemyStates.HIT);
        }

        [MenuItem("GameObject/RoguetyCraft/EnemyController", priority = 1)]
        static void Create()
        {
            GameObject root = new GameObject("EnemyController");
            root.transform.position = new Vector3(0, 0, 0);

            root.AddComponent<EnemyController>();
            root.AddComponent<EnemyMovement>();
            root.AddComponent<EnemyAnimator>();

            GameObject spriteGO = new GameObject("SpriteRenderer", typeof(SpriteRenderer), typeof(Animator));
            spriteGO.transform.parent = root.transform;

            GameObject colliderGO = new GameObject("Collider", typeof(BoxCollider2D));
            colliderGO.transform.parent = root.transform;
        }
    }
}