using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;
using RoguetyCraft.Player.Controller;

namespace RoguetyCraft.Player.Effects
{
    [System.Serializable]
    public class ParticleEffect
    {
        public ParticleSystem ParticleSystem = null;
        public UnityEvent OnStart;
        public UnityEvent OnEnd;
    }

    public class PlayerEffects : MonoBehaviour
    {
        [Foldout("Init Scene Particles", true)]
        public bool HasInitParticles = false;
        [ConditionalField(nameof(HasInitParticles))] public ParticleEffect InitParticles = new();

        [Foldout("Move Particles", true)]
        public bool HasMoveParticles = false;
        [ConditionalField(nameof(HasMoveParticles))] public ParticleEffect MoveParticles = new();

        [Foldout("Turn Particles", true)]
        public bool HasTurnParticles = false;
        [ConditionalField(nameof(HasTurnParticles))] public ParticleEffect TurnParticles = new();

        [Foldout("Jump Particles", true)]
        public bool HasJumpParticles = false;
        [ConditionalField(nameof(HasJumpParticles))] public ParticleEffect JumpParticles = new();

        [Foldout("Land Particles", true)]
        public bool HasLandParticles = false;
        [ConditionalField(nameof(HasLandParticles))] public ParticleEffect LandParticles = new();
        
        [Foldout("Hit Particles", true)]
        public bool HasHitParticles = false;
        [ConditionalField(nameof(HasHitParticles))] public ParticleEffect HitParticles = new();

        private PlayerController _controller;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (HasMoveParticles && _controller.PlayerMovement.IsRunning)
            {
                MoveParticles.OnStart.Invoke();
            }
        }
    }
}
