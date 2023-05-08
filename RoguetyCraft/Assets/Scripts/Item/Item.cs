using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

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
        void OnInteract();
    }
    public interface IUsable
    {
        void OnUse();
    }
    public abstract class Item : ScriptableObject
    {
        public ItemType Type => _type;

        [Separator("Item settings")]
        [SerializeField, ReadOnly] protected ItemType _type;
        [SerializeField] protected Sprite _sprite;

        public Item(Sprite sprite)
        {
            _sprite = sprite;
        }
    }
}
