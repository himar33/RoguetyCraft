using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using static UnityEditor.Experimental.GraphView.GraphView;
using RoguetyCraft.Player.Movement;
using RoguetyCraft.Player.Animation;
using RoguetyCraft.Player.Gun;

namespace RoguetyCraft.Player.Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }
        public PlayerGun PlayerGun { get; private set; }

        public bool IsInteracting => _isInteracting;

        private bool _isInteracting = false;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerAnimator = GetComponentInChildren<PlayerAnimator>();
            PlayerGun = GetComponentInChildren<PlayerGun>();
        }

        private void Update()
        {
            _isInteracting = Input.GetKeyDown(KeyCode.E);
        }
    }
}