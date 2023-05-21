using RoguetyCraft.Items.Generic;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using RoguetyCraft.Player.Controller;
using RoguetyCraft.Items.Controller;

namespace RoguetyCraft.Items.Weapon
{
    [CreateAssetMenu(fileName = "weapon", menuName = "RoguetyCraft/Items/Weapon", order = 1)]
    public class ItemWeapon : Item
    {
        [Separator("Weapon Settings")]
        public float AttackDamage = 10f;
        public float AttackSpeed = 1f;
        public float BulletSpeed = 1f;

        [Separator("Bullet Particle Settings")]
        public Color BulletColor = Color.white;
        public float BulletSize = 1f;
        public float ColliderRadius = 1f;
        public List<Sprite> AnimationSprites;

        [Separator("Bullet Hit Particle Settings")]
        public Color BulletHitColor = Color.white;
        public List<Sprite> HitAnimationSprites;

        public ItemWeapon(Sprite sprite) : base(sprite)
        {
            _type = ItemType.WEAPON;
        }
        public override void OnInteract(ItemController controller)
        {
            PlayerController.Instance.PlayerGun.SetWeapon(this);
            Destroy(controller.gameObject);
        }
        public override void OnUse(ItemController controller)
        {
        }
    }
}
