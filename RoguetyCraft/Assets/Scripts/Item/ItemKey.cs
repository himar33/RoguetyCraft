using RoguetyCraft.Items.Controller;
using RoguetyCraft.Items.Generic;
using RoguetyCraft.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Items.Key
{
    [CreateAssetMenu(fileName = "key", menuName = "RoguetyCraft/Items/Key")]
    public class ItemKey : Item
    {
        protected ItemKey(Sprite sprite) : base(sprite) { }
        protected override void Awake()
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
