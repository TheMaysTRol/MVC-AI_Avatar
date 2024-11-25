using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Controls animations for the avatar during various chat states, such as thinking, responding, and idle.
    /// </summary>
    public class AnimationsController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private AnimationsView animationsView;  /// Reference to the AnimationsView that handles the actual animation playback.
        [SerializeField] private AnimationsView animationsModel; /// This seems to be a duplicate or model reference, not currently used.

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when the avatar starts thinking (typically triggered by user input).
        /// </summary>
        /// <param name="sender">The sender of the message (e.g., the user or AI).</param>
        /// <param name="message">The content of the message that triggered the thinking state.</param>
        public void OnThinkingStart(ChatView.Sender sender, string message)
        {
            animationsView.PlayThinkingAnimation();  /// Play the thinking animation in the AnimationsView.
        }

        /// <summary>
        /// Called when the AI's response is received and ready to be displayed.
        /// </summary>
        /// <param name="sender">The sender of the message (expected to be the AI in this case).</param>
        /// <param name="message">The response message from the AI.</param>
        public void OnResponseReceived(ChatView.Sender sender, string message)
        {
            if (sender == ChatView.Sender.Ai)
            {
                animationsView.PlayResponseAnimation();  /// Play the response animation when the AI responds.
            }
        }

        /// <summary>
        /// Called when the response animation is completed and the avatar should return to idle state.
        /// </summary>
        public void OnResponseCompleted(ChatView.Sender sender)
        {
            if (sender == ChatView.Sender.Ai)
            {
                animationsView.PlayIdleAnimation();  /// Play the idle animation after the response is completed.

            }
        }

        #endregion
    }
}
