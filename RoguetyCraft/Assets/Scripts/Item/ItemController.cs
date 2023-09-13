using UnityEngine;
using MyBox;
using RoguetyCraft.Items.Generic;
using Unity.VisualScripting;
using RoguetyCraft.Player.Controller;
using UnityEditor;

namespace RoguetyCraft.Items.Controller
{
    /// <summary>
    /// Controller class to manage item behavior in the game.
    /// </summary>
    public class ItemController : MonoBehaviour
    {
        #region Fields and Properties

        /// <summary>
        /// Gets the Item associated with this controller.
        /// </summary>
        public Item Item => _item;

        [Separator("Item settings")]
        [SerializeField] private Item _item;

        [Separator("Collider settings")]
        [SerializeField] private Vector3 _colOffset = Vector3.zero;
        [SerializeField] private float _colRadius = 0.5f;
        [SerializeField] private LayerMask _colLayer;
        [SerializeField] private Color _gizmosColor = Color.red;

        [Separator("Sprite settings")]
        [SerializeField] private Color _spriteColor = Color.white;
        [SerializeField, SpriteLayer] private int _spriteLayer = 0;
        [SerializeField] private int _orderLayer = 0;

        [Separator("Animation settings")]
        [SerializeField] private float _animFreq = 8f;
        [SerializeField] private float _animAmp = 0.1f;
        [SerializeField] private Vector3 _animDirection = Vector3.up;
        [SerializeField] private AnimationCurve _animCurve;

        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _startPos;

        #endregion

        #region Unity Lifecycle Methods

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position + _colOffset, _colRadius);
        }

        private void Start()
        {
            InitializeComponents();
            _startPos = transform.position;
        }

        private void Update()
        {
            UpdateAnimation();
            CheckInteraction();
        }

        #endregion

        #region Private Methods

        /// Initializes necessary components like Collider2D and SpriteRenderer.
        private void InitializeComponents()
        {
            bool hasCollider = TryGetComponent(out _collider);
            if (!hasCollider)
            {
                CircleCollider2D col = transform.AddComponent<CircleCollider2D>();
                col.radius = _colRadius;
                col.offset = _colOffset;
                col.isTrigger = true;

                _collider = col;
            }

            bool hasRenderer = TryGetComponent(out _spriteRenderer);
            if (!hasRenderer)
            {
                SpriteRenderer spr = transform.AddComponent<SpriteRenderer>();
                spr.sprite = _item.Sprite;
                spr.color = _spriteColor;
                spr.sortingLayerID = _spriteLayer;
                spr.sortingOrder = _orderLayer;

                _spriteRenderer = spr;
            }
        }

        /// Updates the animation of the item based on time and settings.
        private void UpdateAnimation()
        {
            float t = Mathf.Sin(Time.time * _animFreq);
            float normalizedT = (t + 1f) / 2f;
            transform.position = _startPos + _animDirection * _animCurve.Evaluate(normalizedT) * _animAmp;
        }

        /// Checks for interactions with the player and triggers the item's effect if applicable.
        private void CheckInteraction()
        {
            Collider2D col = Physics2D.OverlapCircle(_collider.bounds.center, _colRadius, _colLayer);
            if (col != null && PlayerController.Instance.IsInteracting)
            {
                _item.OnInteract(this);
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Editor utility for creating a new ItemController GameObject.
        /// </summary>
        [MenuItem("GameObject/RoguetyCraft/ItemController", priority = 1)]
        static void Create()
        {
            GameObject go = new GameObject("NewItem");
            go.transform.position = new Vector3(0, 0, 0);

            go.AddComponent<ItemController>();
        }

        #endregion
    }

}
