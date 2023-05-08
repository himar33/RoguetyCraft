using RoguetyCraft.Items.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

namespace RoguetyCraft.Items.Weapon
{
    [CreateAssetMenu(fileName = "weapon", menuName = "RoguetyCraft/Items/Weapon", order = 1)]
    public class ItemWeapon : Item, IInteractable, IUsable
    {
        [Separator("Weapon settings")]
        public float AttackDamage = 10f;
        public float AttackSpeed = 1f;
        public float BulletSpeed = 1f;

        [Separator("Particle system settings")]
        public Color BulletColor = Color.white;
        public float BulletSize = 1f;
        public List<Sprite> AnimationSprites;
        public List<Sprite> HitAnimationSprites;

        public ItemWeapon(Sprite sprite) : base(sprite)
        {
            _type = ItemType.WEAPON;
        }
        public void OnInteract()
        {
        }
        public void OnUse()
        {
        }
    }
}
