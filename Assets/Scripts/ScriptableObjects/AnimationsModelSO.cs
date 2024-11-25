using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// ScriptableObject that holds animation data for the avatar, including types of animations for different states (Idle, Talk, Think).
    /// Allows customization of how animations are played for each avatar state.
    /// </summary>
    [CreateAssetMenu(fileName = "AnimationData", menuName = "KJAvatar/AnimationData", order = 0)]
    public class AnimationsModelSO : ScriptableObject
    {
        #region Enums

        /// <summary>
        /// Enum representing different animation play types.
        /// </summary>
        public enum AnimationsType
        {
            /// <summary>
            /// Play multiple animations randomly.
            /// </summary>
            PlayMultipleRandom,

            /// <summary>
            /// Play animations in a specific order.
            /// </summary>
            PlayInOrder,

            /// <summary>
            /// Loop one animation randomly chosen from a set.
            /// </summary>
            LoopOneRandom
        }

        #endregion

        #region Serialized Fields

        /// <summary>
        /// Defines how the Idle animation is played.
        /// </summary>
        [Tooltip("Specifies how the Idle animation should be played.")]
        public AnimationsType idleType;

        /// <summary>
        /// Defines how the Talk animation is played.
        /// </summary>
        [Tooltip("Specifies how the Talk animation should be played.")]
        public AnimationsType talkType;

        /// <summary>
        /// Defines how the Think animation is played.
        /// </summary>
        [Tooltip("Specifies how the Think animation should be played.")]
        public AnimationsType thinkType;

        #endregion
    }
}
