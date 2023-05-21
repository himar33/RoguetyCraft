using RoguetyCraft.Items.Controller;
using RoguetyCraft.Items.Generic;
using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Items.Weapon
{
    public class ItemKey : Item
    {
        public ItemKey(Sprite sprite) : base(sprite)
        {
            _type = ItemType.KEY;
        }
        public override void OnInteract(ItemController controller)
        {
        }
        public override void OnUse(ItemController controller)
        {
        }
    }
}
