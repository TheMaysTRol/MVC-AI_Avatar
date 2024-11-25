using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Represents the model for handling animation configurations.
    /// Stores and retrieves animation types based on the configuration settings.
    /// </summary>
    public class AnimationsModel : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private AnimationsModelSO config; /// The configuration object containing different animation types.

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the AnimationsModel with a specific configuration.
        /// </summary>
        /// <param name="config">The AnimationsModelSO configuration that defines the animation types.</param>
        public AnimationsModel(AnimationsModelSO config)
        {
            this.config = config;  /// Assign the provided configuration to the class field.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the idle animation type defined in the configuration.
        /// </summary>
        /// <returns>The animation type for idle state.</returns>
        public AnimationsModelSO.AnimationsType GetIdleAnimationsType() => config.idleType;

        /// <summary>
        /// Retrieves the thinking animation type defined in the configuration.
        /// </summary>
        /// <returns>The animation type for thinking state.</returns>
        public AnimationsModelSO.AnimationsType GetThinkAnimationsType() => config.thinkType;

        /// <summary>
        /// Retrieves the talking animation type defined in the configuration.
        /// </summary>
        /// <returns>The animation type for talking state.</returns>
        public AnimationsModelSO.AnimationsType GetTalkAnimationsType() => config.talkType;

        #endregion
    }
}
