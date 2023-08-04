using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoguetyCraft.Generic.Utility;
using UnityEditor.Animations;

namespace RoguetyCraft.Generic.Animation
{
    [System.Serializable]
    public struct AnimationClipVisual
    {
        [HideInInspector] public string KeyName;
        public AnimationClip ValueClip;
        public AnimationClipVisual(string _name, AnimationClip _clip)
        {
            KeyName = _name;
            ValueClip = _clip;
        }
    }
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] protected AnimatorController _animatorController;
        [SerializeField] protected List<AnimationClipVisual> _clips = new();

        [ButtonMethod]
        public void ResetClips()
        {
            if (!RoguetyUtilities.GetComponent(gameObject, out _animator))
            {
                Debug.LogError("Animator value is null, make sure you attach an animator!");
                return;
            }

            RuntimeAnimatorController runAnim = _animatorController;

            _clips.Clear();
            for (int i = 0; i < runAnim.animationClips.Length; i++)
            {
                AnimationClipVisual clipVisual = new(runAnim.animationClips[i].name, runAnim.animationClips[i]);
                _clips.Add(clipVisual);
            }
        }

        protected Animator _animator;
        protected AnimatorOverrideController _animatorOverride;
        protected AnimationClipOverrides _animatorClips;

        protected virtual void Awake()
        {
            if (!RoguetyUtilities.GetComponent(gameObject, out _animator))
            {
                Debug.LogError("Animator value is null, make sure you attach an animator!");
                return;
            }

            _animatorOverride = new AnimatorOverrideController(_animatorController);
            _animator.runtimeAnimatorController = _animatorOverride;

            _animatorClips = new AnimationClipOverrides(_animatorOverride.overridesCount);
            _animatorOverride.GetOverrides(_animatorClips);

            for (int i = 0; i < _animatorClips.Count; i++)
            {
                _animatorClips[_animatorClips.ElementAt(i).Key.name] = _clips.Find(x => x.ValueClip == _animatorClips.ElementAt(i).Value).ValueClip;
            }
            _animatorOverride.ApplyOverrides(_animatorClips);
        }
    }
}
