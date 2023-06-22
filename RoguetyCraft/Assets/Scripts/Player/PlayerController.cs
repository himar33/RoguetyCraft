using UnityEngine;
using RoguetyCraft.Player.Movement;
using RoguetyCraft.Player.Animation;
using RoguetyCraft.Player.Gun;
using RoguetyCraft.DesignPatterns.Singleton;
using MyBox;

namespace RoguetyCraft.Player.Controller
{
    [RequireTag("Player"), RequireLayer("Player"), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerGun))]
    public class PlayerController : RCSingleton<PlayerController>
    {
        [Foldout("Player Movement", true)]
        public bool HasMovement = true;
        [ConditionalField(nameof(HasMovement)), DisplayInspector(displayScriptField: false)] public PlayerMovement PlayerMovement;

        [Foldout("Player Gun", true)]
        public bool HasGun = true;
        [ConditionalField(nameof(HasGun)), DisplayInspector(displayScriptField: false)] public PlayerGun PlayerGun;

        [Foldout("Player Animator", true)]
        public bool HasAnimator = true;
        [ConditionalField(nameof(HasAnimator)), DisplayInspector(displayScriptField: false)] public PlayerAnimator PlayerAnimator;

        public bool IsInteracting => _isInteracting;

        private bool _isInteracting = false;

        private void Update()
        {
            _isInteracting = Input.GetKeyDown(KeyCode.E);
        }

        private void Reset()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerGun = GetComponent<PlayerGun>();
            PlayerAnimator = gameObject.GetComponentInChildren<PlayerAnimator>();
        }

        private void OnValidate()
        {
            if (gameObject.HasComponent<PlayerMovement>() && PlayerMovement != null) PlayerMovement.enabled = HasMovement;

            if (gameObject.HasComponent<PlayerGun>() && PlayerGun != null) PlayerGun.enabled = HasGun;

            if (gameObject.GetComponentInChildren<PlayerAnimator>() != null && PlayerAnimator != null) PlayerAnimator.enabled = HasAnimator;
        }
    }
}