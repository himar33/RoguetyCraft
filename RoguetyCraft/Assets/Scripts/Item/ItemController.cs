using UnityEngine;
using MyBox;
using RoguetyCraft.Items.Generic;
using Unity.VisualScripting;
using RoguetyCraft.Player.Controller;

namespace RoguetyCraft.Items.Controller
{
    public class ItemController : MonoBehaviour
    {
        public Item Item => _item;

        [Separator("Item settings")]
        [SerializeField] private Item _item;

        [Separator("Collider settings")]
        [SerializeField] private Vector3 _colOffset = Vector3.zero;
        [SerializeField] private float _colRadius = 1f;
        [SerializeField] private LayerMask _colLayer;
        [SerializeField] private Color _gizmosColor = Color.red;

        [Separator("Item settings")]
        [SerializeField] private Color _spriteColor = Color.white;
        [SerializeField, SpriteLayer] private int _spriteLayer = 0;
        [SerializeField] private int _orderLayer = 0;

        [Separator("Animation settings")]
        [SerializeField] private float _animFreq = 1f;
        [SerializeField] private float _animAmp = 1f;
        [SerializeField] private Vector3 _animDirection = Vector3.zero;
        [SerializeField] private AnimationCurve _animCurve;

        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _startPos;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position + _colOffset, _colRadius);
        }

        private void Start()
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

            _startPos = transform.position;
        }

        private void Update()
        {
            UpdateAnimation();
            CheckInteraction();
        }

        private void UpdateAnimation()
        {
            float t = Mathf.Sin(Time.time * _animFreq);
            float normalizedT = (t + 1f) / 2f;
            transform.position = _startPos + _animDirection * _animCurve.Evaluate(normalizedT) * _animAmp;
        }

        private void CheckInteraction()
        {
            Collider2D col = Physics2D.OverlapCircle(_collider.bounds.center, _colRadius, _colLayer);
            if (col != null && PlayerController.Instance.IsInteracting)
            {
                _item.OnInteract(this);
            }
        }
    }
}
