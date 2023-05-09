using UnityEngine;
using MyBox;
using RoguetyCraft.Player.Controller;
using RoguetyCraft.Items.Controller;

namespace RoguetyCraft.Items.Generic
{
    public enum ItemType
    {
        WEAPON,
        CONSUMABLE,
        KEY,
        POWERUP
    }
    public interface IInteractable
    {
        void OnInteract(ItemController controller);
    }
    public interface IUsable
    {
        void OnUse(ItemController controller);
    }
    public abstract class Item : ScriptableObject, IInteractable, IUsable
    {
        public ItemType Type => _type;
        public Sprite Sprite => _sprite;

        [Separator("Item settings")]
        [SerializeField, ReadOnly] protected ItemType _type;
        [SerializeField] protected Sprite _sprite;

        public Item(Sprite sprite) { _sprite = sprite; }

        public virtual void OnInteract(ItemController controller) { }
        public virtual void OnUse(ItemController controller) { }
    }
}
