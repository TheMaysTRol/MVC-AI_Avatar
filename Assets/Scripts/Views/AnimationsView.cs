using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Manages the animations by interacting with the Animator component.
    /// Responsible for playing specific animation states such as thinking, responding, and idle.
    /// </summary>
    public class AnimationsView : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Animator animator; /// Reference to the Animator component used to play animations.

        #endregion

        #region Public Methods

        /// <summary>
        /// Plays the thinking animation in the Animator.
        /// This is typically triggered when the AI is processing a response.
        /// </summary>
        public void PlayThinkingAnimation()
        {
            animator.SetFloat("Blend", 1f);  /// Triggers the "Thinking" animation state in the Animator.
        }

        /// <summary>
        /// Plays the responding animation in the Animator.
        /// This animation is played when the AI is responding to the user.
        /// </summary>
        public void PlayResponseAnimation()
        {
            animator.SetFloat("Blend", 0.5f);  /// Triggers the "Responding" animation state in the Animator.
        }

        /// <summary>
        /// Plays the idle animation in the Animator.
        /// This animation is played when the AI is in an idle state.
        /// </summary>
        public void PlayIdleAnimation()
        {
            animator.SetFloat("Blend", 0);
        }

        #endregion
    }
}
