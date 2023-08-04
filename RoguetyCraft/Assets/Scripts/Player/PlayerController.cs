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
            HasMovement = RoguetyUtilities.GetComponent(gameObject, out PlayerMovement);

            HasGun = RoguetyUtilities.GetComponent(gameObject, out PlayerGun);

            HasAnimator = RoguetyUtilities.GetComponent(gameObject, out PlayerAnimator);
        }

        private void OnValidate()
        {
            bool hasMove = RoguetyUtilities.GetComponent(gameObject, out PlayerMovement);
            if (hasMove) PlayerMovement.enabled = HasMovement;
            else if (HasMovement && !hasMove) Debug.LogWarning("Player Controller object needs a Player Movement script attached to do this action!");

            bool hasGun = RoguetyUtilities.GetComponent(gameObject, out PlayerGun);
            if (hasGun) PlayerGun.enabled = HasGun;
            else if (HasGun && !hasGun) Debug.LogWarning("Player Controller object needs a Player Gun script attached to do this action!");

            bool hasAnim = RoguetyUtilities.GetComponent(gameObject, out PlayerAnimator);
            if (hasAnim) PlayerAnimator.enabled = HasAnimator;
            else if (HasAnimator && !hasAnim) Debug.LogWarning("Player Controller object needs a Player Animator script attached to do this action!");
        }
    }
}