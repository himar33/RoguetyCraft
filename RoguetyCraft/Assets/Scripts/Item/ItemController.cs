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
        [SerializeField, Tag] private string _colTag;
        [SerializeField] private Vector3 _colOffset = Vector3.zero;
        [SerializeField] private float _colRadius = 1f;
        [SerializeField] private LayerMask _colLayer;
        [SerializeField] private Color _gizmosColor = Color.red;

        [Separator("Animation settings")]
        [SerializeField] private float _animSpeed = 1f;
        [SerializeField] private Vector3 _animDirection = Vector3.zero;
        [SerializeField] private AnimationCurve _animCurve;

        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;

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

                _spriteRenderer = spr;
            }
        }

        private void Update()
        {
            UpdateAnimation();
            CheckInteraction();
        }

        private void UpdateAnimation()
        {
            transform.position += _animDirection.normalized * _animSpeed * _animCurve.Evaluate(Time.time) * Time.deltaTime;
        }

        private void CheckInteraction()
        {
            Physics2D.OverlapCircle(_collider.bounds.center, _colRadius, _colLayer);
            if (_collider != null && PlayerController.Instance.IsInteracting)
            {
                _item.OnInteract(this);
            }
        }
    }
}
