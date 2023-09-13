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
    /// <summary>
    /// Manages the behavior and state of an enemy entity in the game.
    /// </summary>
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyAnimator))]
    [RequireLayer("Enemy")]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        #region Public Properties

        /// <summary>
        /// Gets the EnemyMovement component associated with this enemy.
        /// </summary>
        public EnemyMovement EMovement => _enemyMovement;

        /// <summary>
        /// Gets the EnemyAnimator component associated with this enemy.
        /// </summary>
        public EnemyAnimator EAnimator => _enemyAnimator;

        /// <summary>
        /// Gets the state machine managing this enemy's states.
        /// </summary>
        public EnemyStateMachine EStateMachine { get; private set; }

        /// <summary>
        /// Gets or sets the current health of the enemy.
        /// </summary>
        public float Health { get => _health; set => _health = value; }

        /// <summary>
        /// Gets or sets the hit time for the enemy.
        /// </summary>
        public float HitTime { get => _hitTime; set => _hitTime = value; }

        /// <summary>
        /// Gets or sets the color mask when the enemy is hit.
        /// </summary>
        public Color HitColorMask { get => _hitColorMask; set => _hitColorMask = value; }

        /// <summary>
        /// Gets or sets the initial color mask of the enemy.
        /// </summary>
        public Color InitialColorMask { get => _initialColorMask; set => _initialColorMask = value; }

        /// <summary>
        /// Gets or sets the last recorded velocity of the enemy.
        /// </summary>
        public float LastVelocity { get => _lastVelocity; set => _lastVelocity = value; }

        #endregion

        #region Serialized Fields

        [SerializeField] private Enemy _enemyData;

        #endregion

        #region Private Fields

        private float _health;
        private float _hitTime;
        private Color _hitColorMask;
        private Color _initialColorMask;
        private float _lastVelocity = 0f;

        private EnemyMovement _enemyMovement;
        private EnemyAnimator _enemyAnimator;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region Unity Lifecycle Methods

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

        #endregion

        #region Validation and Utility Methods

        private void OnValidate()
        {
            if (_enemyData != null)
            {
                _health = _enemyData.Health;
                _hitTime = _enemyData.HitTime;
                _hitColorMask = _enemyData.HitColorMask;
            }
        }

        #endregion

        #region IDamageable Implementation

        /// <summary>
        /// Applies damage to the enemy and updates its state accordingly.
        /// </summary>
        public void TakeDamage(float amount)
        {
            Health -= amount;
            EStateMachine.Set(Health <= 0 ? EnemyStates.DEAD : EnemyStates.HIT);
        }

        #endregion

        #region Editor Utilities

        [MenuItem("GameObject/RoguetyCraft/EnemyController", priority = 1)]
        static void Create()
        {
            GameObject root = new GameObject("EnemyController");
            root.transform.position = Vector3.zero;

            root.AddComponent<EnemyController>();
            root.AddComponent<EnemyMovement>();
            root.AddComponent<EnemyAnimator>();

            GameObject spriteGO = new GameObject("SpriteRenderer", typeof(SpriteRenderer), typeof(Animator));
            spriteGO.transform.parent = root.transform;

            GameObject colliderGO = new GameObject("Collider", typeof(BoxCollider2D));
            colliderGO.transform.parent = root.transform;
        }

        #endregion
    }
}