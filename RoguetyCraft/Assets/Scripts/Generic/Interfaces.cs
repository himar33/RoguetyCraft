using RoguetyCraft.Items.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Generic.Interfaces
{
    /// <summary>
    /// Represents objects that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Defines the behavior when an object is interacted with.
        /// </summary>
        /// <param name="controller">The ItemController that initiates the interaction.</param>
        void OnInteract(ItemController controller);
    }

    /// <summary>
    /// Represents objects that can be used.
    /// </summary>
    public interface IUsable
    {
        /// <summary>
        /// Defines the behavior when an object is used.
        /// </summary>
        /// <param name="controller">The ItemController that initiates the use.</param>
        void OnUse(ItemController controller);
    }

    /// <summary>
    /// Represents objects that can take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Gets or sets the current health.
        /// </summary>
        float Health { get; set; }

        /// <summary>
        /// Gets or sets the time of the last hit.
        /// </summary>
        float HitTime { get; set; }

        /// <summary>
        /// Gets or sets the color mask used when hit.
        /// </summary>
        Color HitColorMask { get; set; }

        /// <summary>
        /// Defines the behavior when the object takes damage.
        /// </summary>
        /// <param name="amount">The amount of damage to take.</param>
        void TakeDamage(float amount);
    }

    /// <summary>
    /// Represents objects that can deal damage.
    /// </summary>
    public interface IDamager
    {
        /// <summary>
        /// Gets or sets the amount of damage dealt.
        /// </summary>
        float AttackDamage { get; set; }

        /// <summary>
        /// Defines the behavior when the object deals damage to a target.
        /// </summary>
        /// <param name="target">The IDamageable object to deal damage to.</param>
        void DealDamage(IDamageable target);
    }
}
