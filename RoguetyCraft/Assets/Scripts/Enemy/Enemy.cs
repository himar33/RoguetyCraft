using UnityEngine;
using MyBox;
using RoguetyCraft.Generic.Animation;
using System.Collections.Generic;
using UnityEditor.Animations;
using Cinemachine;

namespace RoguetyCraft.Enemies.Generic
{
    [CreateAssetMenu(fileName = "enemy", menuName = "RoguetyCraft/Enemy")]
    public class Enemy : ScriptableObject
    {
        [Separator("Enemy General")]
        public float Health = 10f;
        public float HitTime = 0.5f;
        public Color HitColorMask = Color.red;

        [Space(20)]

        [Separator("Enemy Movement")]
        public float MoveSpeed = 3f;
        public float ChaseMoveSpeed = 6f;
        public float ChangeStopTime = 2f;

        [Space(20)]
        public float Distance = 0.2f;
        public LayerMask Groundlayer;

        [Space(20)]
        [Tag] public string TargetTag;
        public float TargetDistance = 4f;
        public float AttackDistance = 1f;
        public LayerMask TargetLayer;

        [Space(20)]

        [Separator("Enemy Animator")]
        public AnimatorController AnimatorController;
        public List<AnimationClipVisual> Clips = new();

        private AnimatorController _animatorController;

        private void OnValidate()
        {
            if (_animatorController != AnimatorController)
            {
                _animatorController = AnimatorController;

                RuntimeAnimatorController runAnim = AnimatorController;

                Clips.Clear();
                for (int i = 0; i < runAnim.animationClips.Length; i++)
                {
                    AnimationClipVisual clipVisual = new(runAnim.animationClips[i].name, runAnim.animationClips[i]);
                    Clips.Add(clipVisual);
                }
            }
        }
    }
}
