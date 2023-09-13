using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoguetyCraft.Generic.Utility;
using UnityEditor.Animations;

namespace RoguetyCraft.Generic.Animation
{
    [System.Serializable]
    public class AnimationClipVisual
    {
        [ReadOnly] public string KeyName;
        public AnimationClip ValueClip;

        /// <summary>
        /// Constructs an AnimationClipVisual object with the given name and clip.
        /// </summary>
        /// <param name="name">The key name for the animation clip.</param>
        /// <param name="clip">The animation clip.</param>
        public AnimationClipVisual(string name, AnimationClip clip)
        {
            KeyName = name;
            ValueClip = clip;
        }
    }

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        /// <summary>
        /// Initializes the AnimationClipOverrides list with the given capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        /// <summary>
        /// Accessor for animation clip by name.
        /// </summary>
        /// <param name="name">The name of the animation clip.</param>
        /// <returns>The animation clip with the specified name.</returns>
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
        public AnimatorController AnimatorController
        {
            get { return _animatorController; }
            set { _animatorController = value; }
        }
        public List<AnimationClipVisual> Animations
        {
            get { return _animationClips; }
            set { _animationClips = value; }
        }

        [SerializeField] protected AnimatorController _animatorController;
        [SerializeField] protected List<AnimationClipVisual> _animationClips = new();

        protected Animator _animator;
        protected AnimatorOverrideController _animatorOverride;
        protected AnimationClipOverrides _animatorClips;

        /// <summary>
        /// Initializes the sprite animator and applies animation overrides if specified.
        /// </summary>
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
                _animatorClips[_animatorClips.ElementAt(i).Key.name] = _animationClips.Find(x => x.KeyName == _animatorClips.ElementAt(i).Key.name).ValueClip;
            }
            _animatorOverride.ApplyOverrides(_animatorClips);
        }
    }
}
