using MyBox;
using RoguetyCraft.Items.Weapon;
using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public ItemWeapon Weapon => _weapon;
    public bool IsShooting => _isShooting;

    [Separator("Weapon settings")]
    [SerializeField] private ItemWeapon _weapon;

    [Separator("Particle system settings")]
    [SerializeField] private ParticleSystemShapeType _shapeType = ParticleSystemShapeType.SingleSidedEdge;
    [SerializeField] private float _shapeRadius = 0f;
    [SerializeField, Range(12, 60)] private float _animationSpeed = 12f;

    [Separator("Bullet Offset")]
    [SerializeField] private Vector3 _bulletPoint = Vector3.zero;
    [SerializeField] private float _gizmoRadius = 0.2f;
    [SerializeField] private Color _gizmoColor = Color.red;

    [ButtonMethod]
    public void SetParticlePosition()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.position = transform.position + _bulletPoint;
    }

    private bool _isShooting = false;
    private PlayerController _player;
    private ParticleSystem _particleS;
    private ParticleSystemRenderer _particleSRenderer;
    private float _initialSpeed;
    private float _initialRate;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
        _particleS = GetComponentInChildren<ParticleSystem>();
        _particleSRenderer = GetComponentInChildren<ParticleSystemRenderer>();

        var psMain = _particleS.main;
        _initialSpeed = psMain.startSpeed.constant;
        psMain.startSpeed = _initialSpeed * _weapon.BulletSpeed;
        psMain.playOnAwake = true;

        var psAnim = _particleS.textureSheetAnimation;
        psAnim.enabled = true;
        psAnim.fps = _animationSpeed;
        for (int i = psAnim.spriteCount - 1; i > 0; i--)
        {
            psAnim.RemoveSprite(i);
        }
        for (int i = 0; i < _weapon.AnimationSprites.Count; i++)
        {
            if (psAnim.spriteCount - 1 >= i)
            {
                psAnim.SetSprite(i, _weapon.AnimationSprites[i]);
            }
            else psAnim.AddSprite(_weapon.AnimationSprites[i]);
        }

        var psShape = _particleS.shape;
        psShape.enabled = true;
        psShape.shapeType = _shapeType;
        psShape.radius = _shapeRadius;

        var psEmission = _particleS.emission;
        psEmission.enabled = true;
        _initialRate = psEmission.rateOverTime.constant;
        psEmission.rateOverTime = _initialRate * _weapon.AttackSpeed;

        if (_weapon.HitAnimationSprites.Count > 0)
        {
            var hitAnim = _weapon.HitAnimationSprites;
            ParticleSystem subEmitter = new();

            var subEMain = subEmitter.main;
            subEMain.startLifetime = (hitAnim.Count - 1) / 10;

            var psSubEmitters = _particleS.subEmitters;
            psSubEmitters.enabled = true;
            psSubEmitters.AddSubEmitter(subEmitter, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);
        }
    }

    private void Update()
    {
        GatherInput();
        SetDirection(_player.PDirection.x);
    }

    public void GatherInput()
    {
        var psEmission = _particleS.emission;

        if (Input.GetKey(KeyCode.E))
        {
            _isShooting = true;
            psEmission.enabled = true;
        }
        else
        {
            _isShooting = false;
            psEmission.enabled = false;
        }
    }

    public void SetDirection(float dir)
    {
        var psShape = _particleS.shape;
        psShape.rotation = new Vector3(0, 0, (dir > 0) ? -90 : 90);
        
        _particleSRenderer.flip = new Vector3((dir > 0) ? 0 : 1, 0, 0);

        _particleS.transform.position = transform.position + (_bulletPoint * ((dir > 0) ? 1 : -1));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position + _bulletPoint, _gizmoRadius);
    }
}
