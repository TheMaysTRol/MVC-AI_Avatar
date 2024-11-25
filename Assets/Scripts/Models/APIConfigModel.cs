using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Holds and manages API configuration settings, including API key, URL, and custom messages.
    /// Interacts with an `APIConfigModelSO` object to fetch and set API-related values.
    /// </summary>
    public class APIConfigModel : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private APIConfigModelSO config;  /// Reference to the ScriptableObject containing API configuration.

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the APIConfigModel with a specified APIConfigModelSO.
        /// </summary>
        /// <param name="config">The APIConfigModelSO containing the configuration data.</param>
        public APIConfigModel(APIConfigModelSO config)
        {
            this.config = config;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the API key from the configuration.
        /// </summary>
        /// <returns>The API key stored in the configuration.</returns>
        public string GetApiKey() => config.apiKey;

        /// <summary>
        /// Gets the API URL from the configuration.
        /// </summary>
        /// <returns>The API URL stored in the configuration.</returns>
        public string GetApiUrl() => config.apiUrl;

        /// <summary>
        /// Gets the custom permanent message from the configuration.
        /// </summary>
        /// <returns>The custom permanent message stored in the configuration.</returns>
        public string GetApiCustomPermanentMessage() => config.customPermanentMessage;

        /// <summary>
        /// Sets the API key in the configuration.
        /// </summary>
        /// <param name="key">The new API key to set.</param>
        /// <returns>The updated API key after assignment.</returns>
        public string SetApiKey(string key) => config.apiKey = key;

        /// <summary>
        /// Sets the API URL in the configuration.
        /// </summary>
        /// <param name="url">The new API URL to set.</param>
        /// <returns>The updated API URL after assignment.</returns>
        public string SetApiUrl(string url) => config.apiUrl = url;

        /// <summary>
        /// Sets the custom permanent message in the configuration.
        /// </summary>
        /// <param name="customPermanentMessage">The new custom permanent message to set.</param>
        /// <returns>The updated custom permanent message after assignment.</returns>
        public string SetApiCustomPermanentMessage(string customPermanentMessage) => config.customPermanentMessage = customPermanentMessage;

        #endregion
    }
}
