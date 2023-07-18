using UnityEngine;
using RoguetyCraft.Player.Movement;
using RoguetyCraft.Player.Animation;
using RoguetyCraft.Player.Gun;
using RoguetyCraft.DesignPatterns.Singleton;
using RoguetyCraft.Generic.Utility;
using MyBox;

namespace RoguetyCraft.Player.Controller
{
    [RequireTag("Player"), RequireLayer("Player")]
    public class PlayerController : RCSingleton<PlayerController>
    {
        [Foldout("Player Movement", true)]
        public bool HasMovement = false;
        [ConditionalField(nameof(HasMovement)), DisplayInspector(displayScriptField: false)] public PlayerMovement PlayerMovement;

        [Foldout("Player Gun", true)]
        public bool HasGun = false;
        [ConditionalField(nameof(HasGun)), DisplayInspector(displayScriptField: false)] public PlayerGun PlayerGun;

        [Foldout("Player Animator", true)]
        public bool HasAnimator = false;
        [ConditionalField(nameof(HasAnimator)), DisplayInspector(displayScriptField: false)] public PlayerAnimator PlayerAnimator;

        public bool IsInteracting => _isInteracting;

        private bool _isInteracting = false;

        private void Update()
        {
            _isInteracting = Input.GetKeyDown(KeyCode.E);
        }

        private void Reset()
        {
            HasMovement = Utilities.GetComponent(gameObject, out PlayerMovement);

            HasGun = Utilities.GetComponent(gameObject, out PlayerGun);

            HasAnimator = Utilities.GetComponent(gameObject, out PlayerAnimator);
        }

        private void OnValidate()
        {
            if ((gameObject.HasComponent<PlayerMovement>() || GetComponentInChildren<PlayerMovement>() != null))
            {
                if (PlayerMovement == null)
                {
                    if (TryGetComponent(out PlayerMovement)) {}
                    else PlayerMovement = GetComponentInChildren<PlayerMovement>();
                }
                PlayerMovement.enabled = HasMovement;
            }
            else if (HasMovement) Debug.LogWarning("Player Controller object needs a Player Movement script attached to do this action!");

            if ((gameObject.HasComponent<PlayerGun>() || GetComponentInChildren<PlayerGun>() != null))
            {
                if (PlayerGun == null)
                {
                    if (TryGetComponent(out PlayerGun)) { }
                    else PlayerGun = GetComponentInChildren<PlayerGun>();
                }
                PlayerGun.enabled = HasGun;
            }
            else if (HasGun) Debug.LogWarning("Player Controller object needs a Player Gun script attached to do this action!");

            if ((gameObject.HasComponent<PlayerAnimator>() || GetComponentInChildren<PlayerAnimator>() != null))
            {
                if (PlayerAnimator == null)
                {
                    if (TryGetComponent(out PlayerAnimator)) { }
                    else PlayerAnimator = GetComponentInChildren<PlayerAnimator>();
                }
                PlayerAnimator.enabled = HasAnimator;
            }
            else if (HasAnimator) Debug.LogWarning("Player Controller object needs a Player Animator script attached to do this action!");
        }
    }
}