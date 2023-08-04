using UnityEngine;
using MyBox;
using RoguetyCraft.Player.Controller;
using RoguetyCraft.Items.Controller;
using UnityEditor;
using RoguetyCraft.Generic.Interfaces;

namespace RoguetyCraft.Items.Generic
{
    public enum ItemType
    {
        CONSUMABLE,
        WEAPON,
        KEY,
        POWERUP
    }
    
    public abstract class Item : ScriptableObject, IInteractable, IUsable
    {
        public ItemType Type => _type;
        public Sprite Sprite => _sprite;

        [Separator("Item settings")]
        [SerializeField, ReadOnly] protected ItemType _type;
        [SerializeField] protected Sprite _sprite;

        protected Item(Sprite sprite) { _sprite = sprite; }
        protected virtual void Awake() { }
        public virtual void OnInteract(ItemController controller) { }
        public virtual void OnUse(ItemController controller) { }
    }
}
