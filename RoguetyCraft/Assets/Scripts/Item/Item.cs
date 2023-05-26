using UnityEngine;
using MyBox;
using RoguetyCraft.Player.Controller;
using RoguetyCraft.Items.Controller;
using UnityEditor;

namespace RoguetyCraft.Items.Generic
{
    public enum ItemType
    {
        CONSUMABLE,
        WEAPON,
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

        protected Item(Sprite sprite) { _sprite = sprite; }
        protected virtual void Awake() { }
        public virtual void OnInteract(ItemController controller) { }
        public virtual void OnUse(ItemController controller) { }

        [MenuItem("GameObject/RoguetyCraft/Item", priority = 1)]
        static void Create()
        {
            GameObject go = new GameObject("NewItem");
            go.transform.position = new Vector3(0, 0, 0);

            go.AddComponent<ItemController>();
        }
    }
}
