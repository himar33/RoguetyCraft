using RoguetyCraft.Items.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Generic.Interfaces
{
    public interface IInteractable
    {
        void OnInteract(ItemController controller);
    }
    public interface IUsable
    {
        void OnUse(ItemController controller);
    }
    public interface IDamageable
    {
        public float Health { get; set; }
        public void TakeDamage(float amount);
    }
    public interface IDamager
    {
        public float AttackDamage { get; set; }
        public void DealDamage(IDamageable target);
    }
}
