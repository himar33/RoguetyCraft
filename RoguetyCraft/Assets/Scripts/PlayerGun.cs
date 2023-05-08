using MyBox;
using RoguetyCraft.Items.Weapon;
using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerGun : MonoBehaviour
{
    public ItemWeapon Weapon => _weapon;
    public bool IsShooting => _isShooting;

    [Separator("Weapon settings")]
    [SerializeField] private ItemWeapon _weapon;

    [Separator("Particle system settings")]
    [SerializeField] private ParticleSystemShapeType _shapeType = ParticleSystemShapeType.SingleSidedEdge;
    [SerializeField] private float _shapeRadius = 0f;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField, Range(12, 60)] private float _animationSpeed = 12f;
    [SerializeField] private Material _particlesMaterial;

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

    private PlayerController _player;
    private ParticleSystem _particleS;
    private ParticleSystemRenderer _particleSRenderer;

    private bool _isShooting = false;
    private float _initialSpeed;
    private float _initialRate;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        SetParticleSystemSettings();

        if (_weapon.HitAnimationSprites.Count > 0)
        {
            SetSubEmitterSettings();
        }
    }

    private void SetParticleSystemSettings()
    {
        var particleSGO = new GameObject("Bullet Particles");
        particleSGO.transform.SetParent(transform);

        _particleS = particleSGO.AddComponent<ParticleSystem>();
        _particleSRenderer = particleSGO.GetComponent<ParticleSystemRenderer>();
        _particleSRenderer.material = _particlesMaterial;

        var psMain = _particleS.main;
        _initialSpeed = psMain.startSpeed.constant;
        psMain.startSpeed = _initialSpeed * _weapon.BulletSpeed;
        psMain.startSize = _weapon.BulletSize;
        psMain.playOnAwake = true;
        psMain.simulationSpace = ParticleSystemSimulationSpace.World;
        psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
        psMain.startColor = _weapon.BulletColor;

        var psEmission = _particleS.emission;
        psEmission.enabled = true;
        _initialRate = psEmission.rateOverTime.constant;
        psEmission.rateOverTime = _initialRate * _weapon.AttackSpeed;

        var psShape = _particleS.shape;
        psShape.enabled = true;
        psShape.shapeType = _shapeType;
        psShape.radius = _shapeRadius;

        var psCollision = _particleS.collision;
        psCollision.enabled = true;
        psCollision.type = ParticleSystemCollisionType.World;
        psCollision.mode = ParticleSystemCollisionMode.Collision2D;
        psCollision.lifetimeLoss = 1f;
        psCollision.collidesWith = _hitLayer;

        var psAnim = _particleS.textureSheetAnimation;
        psAnim.enabled = true;
        psAnim.mode = ParticleSystemAnimationMode.Sprites;
        psAnim.timeMode = ParticleSystemAnimationTimeMode.FPS;
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
    }

    private void SetSubEmitterSettings()
    {
        var hitAnim = _weapon.HitAnimationSprites;

        var subEmitterGO = new GameObject("Hit Particles");
        subEmitterGO.transform.SetParent(transform.GetChild(0));

        var subEmitter = subEmitterGO.AddComponent<ParticleSystem>();
        var subRenderer = subEmitterGO.GetComponent<ParticleSystemRenderer>();
        subRenderer.material = _particlesMaterial;

        var subMain = subEmitter.main;
        subMain.startLifetime = (hitAnim.Count - 1) * 0.1f;
        subMain.simulationSpace = ParticleSystemSimulationSpace.World;
        subMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
        subMain.startSize = _weapon.BulletSize;
        subMain.startColor = _weapon.BulletColor;

        var subEmission = subEmitter.emission;
        subEmission.SetBursts(new ParticleSystem.Burst[]
        {
            new Burst(1.0f, 1),
        });

        var subShape = subEmitter.shape;
        subShape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
        subShape.radius = 0f;

        var subAnimation = subEmitter.textureSheetAnimation;
        subAnimation.enabled = true;
        subAnimation.mode = ParticleSystemAnimationMode.Sprites;
        subAnimation.timeMode = ParticleSystemAnimationTimeMode.FPS;
        subAnimation.fps = _animationSpeed;
        for (int i = 0; i < _weapon.HitAnimationSprites.Count; i++)
        {
            if (subAnimation.spriteCount - 1 >= i)
            {
                subAnimation.SetSprite(i, _weapon.HitAnimationSprites[i]);
            }
            else subAnimation.AddSprite(_weapon.HitAnimationSprites[i]);
        }

        var psSubEmitters = _particleS.subEmitters;
        psSubEmitters.enabled = true;
        psSubEmitters.AddSubEmitter(subEmitter, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);
    }

    private void Update()
    {
        GatherInput();
        SetDirection(_player.PDirection.x);
    }

    public void GatherInput()
    {
        var psEmission = _particleS.emission;

        if (_player.IsGrounded && Input.GetKey(KeyCode.E))
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
