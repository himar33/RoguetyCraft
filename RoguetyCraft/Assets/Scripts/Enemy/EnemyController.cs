using RoguetyCraft.Enemies.Movement;
using RoguetyCraft.Generic.Interfaces;
using RoguetyCraft.Generic.Utility;
using UnityEngine;
using UnityEditor;
using RoguetyCraft.Enemies.Generic;
using RoguetyCraft.Player.Movement;

namespace RoguetyCraft.Enemies.Controller
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Enemy _enemyData;
        public Enemy EnemyData => _enemyData;

        public EnemyMovement EMovement => _enemyMovement;
        public EnemyAnimator EAnimator => _enemyAnimator;
        public EnemyStateMachine EStateMachine { get; private set; }
        public float Health { get => _enemyData.Health; set => Health = value; }
        public float HitTime { get => _enemyData.HitTime; set => HitTime = value; }
        public Color HitColorMask { get => _enemyData.HitColorMask; set => HitColorMask = value; }
        public Color InitialColorMask { get => _initialColorMask; set => _initialColorMask = value; }
        public float LastVelocity { get => _lastVelocity; set => _lastVelocity = value; }

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
                if (RoguetyUtilities.GetComponent(gameObject, out EnemyMovement movement))
                {
                    movement.UpdateMovementData(_enemyData);
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