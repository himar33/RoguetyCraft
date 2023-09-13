using UnityEngine;
using MyBox;
using RoguetyCraft.Player.Controller;
using RoguetyCraft.Items.Controller;
using UnityEditor;
using RoguetyCraft.Generic.Interfaces;

namespace RoguetyCraft.Items.Generic
{
    /// <summary>
    /// Enum to represent various types of items.
    /// </summary>
    public enum ItemType
    {
        CONSUMABLE,
        WEAPON,
        KEY,
        POWERUP
    }

    /// <summary>
    /// Abstract base class for all items.
    /// Implements the IInteractable and IUsable interfaces.
    /// </summary>
    public abstract class Item : ScriptableObject, IInteractable, IUsable
    {
        #region Fields and Properties

        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        public ItemType Type => _type;

        /// <summary>
        /// Gets the sprite associated with the item.
        /// </summary>
        public Sprite Sprite => _sprite;

        [Separator("Item settings")]
        [SerializeField, ReadOnly]
        protected ItemType _type; // The type of the item

        [SerializeField]
        protected Sprite _sprite; // The sprite associated with the item

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that initializes the item with a sprite.
        /// </summary>
        /// <param name="sprite">The sprite associated with the item.</param>
        protected Item(Sprite sprite) { _sprite = sprite; }

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Unity's Awake method, called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake() { }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// Called when the item is interacted with.
        /// </summary>
        /// <param name="controller">The ItemController handling the interaction.</param>
        public virtual void OnInteract(ItemController controller) { }

        /// <summary>
        /// Called when the item is used.
        /// </summary>
        /// <param name="controller">The ItemController handling the usage.</param>
        public virtual void OnUse(ItemController controller) { }

        #endregion
    }

}
