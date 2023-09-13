using UnityEngine;
using RoguetyCraft.Player.Movement;
using RoguetyCraft.Player.Animation;
using RoguetyCraft.Player.Gun;
using RoguetyCraft.DesignPatterns.Singleton;
using RoguetyCraft.Generic.Utility;
using MyBox;

namespace RoguetyCraft.Player.Controller
{
    /// <summary>
    /// The PlayerController class is responsible for managing the player's
    /// movement, gun, and animation capabilities.
    /// </summary>
    [RequireTag("Player"), RequireLayer("Player")]
    public class PlayerController : RCSingleton<PlayerController>
    {
        #region Public Fields

        [Foldout("Player Movement", true)]
        public bool HasMovement = false;

        [ConditionalField(nameof(HasMovement)), DisplayInspector(displayScriptField: false)]
        public PlayerMovement PlayerMovement;

        [Foldout("Player Gun", true)]
        public bool HasGun = false;

        [ConditionalField(nameof(HasGun)), DisplayInspector(displayScriptField: false)]
        public PlayerGun PlayerGun;

        [Foldout("Player Animator", true)]
        public bool HasAnimator = false;

        [ConditionalField(nameof(HasAnimator)), DisplayInspector(displayScriptField: false)]
        public PlayerAnimator PlayerAnimator;

        /// <summary>
        /// Read-only property indicating if the player is interacting.
        /// </summary>
        public bool IsInteracting => _isInteracting;

        #endregion

        #region Private Fields

        private bool _isInteracting = false;

        #endregion

        #region Unity Methods

        private void Update()
        {
            // Update interaction state based on player input.
            _isInteracting = Input.GetKeyDown(KeyCode.E);
        }

        /// <summary>
        /// Resets the PlayerController by validating its components.
        /// </summary>
        private void Reset()
        {
            HasMovement = RoguetyUtilities.GetComponent(gameObject, out PlayerMovement);
            HasGun = RoguetyUtilities.GetComponent(gameObject, out PlayerGun);
            HasAnimator = RoguetyUtilities.GetComponent(gameObject, out PlayerAnimator);
        }

        /// <summary>
        /// Validates the PlayerController's state by ensuring all necessary components are attached.
        /// </summary>
        private void OnValidate()
        {
            ValidateComponent(HasMovement, nameof(PlayerMovement), out PlayerMovement);
            ValidateComponent(HasGun, nameof(PlayerGun), out PlayerGun);
            ValidateComponent(HasAnimator, nameof(PlayerAnimator), out PlayerAnimator);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates a specific component in the PlayerController.
        /// </summary>
        private void ValidateComponent<T>(bool hasComponent, string componentName, out T component) where T : MonoBehaviour
        {
            bool found = RoguetyUtilities.GetComponent(gameObject, out component);
            if (found) component.enabled = hasComponent;
            else if (hasComponent && !found)
            {
                Debug.LogWarning($"Player Controller object needs a {componentName} script attached to perform this action!");
            }
        }

        #endregion
    }
}