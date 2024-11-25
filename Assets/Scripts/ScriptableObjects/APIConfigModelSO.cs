using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// ScriptableObject that stores configuration settings for the API connection,
    /// including the API URL, API Key, model selection, and an optional permanent message.
    /// This configuration is used to make requests to the AI API.
    /// </summary>
    [CreateAssetMenu(fileName = "APIConfig", menuName = "KJAvatar/APIConfig", order = 1)]
    public class APIConfigModelSO : ScriptableObject
    {
        #region Serialized Fields

        /// <summary>
        /// The URL for the API endpoint that handles chat completions.
        /// Defaults to a specific URL for Groq's API, but can be customized.
        /// </summary>
        [Header("API Configuration")]
        public string apiUrl = "https://api.groq.com/openai/v1/chat/completions";

        /// <summary>
        /// The API key required for authenticating requests to the API.
        /// This should be securely stored and retrieved from a safe source.
        /// </summary>
        public string apiKey;

        /// <summary>
        /// The model name or identifier to be used for the AI responses.
        /// Defaults to "llama3-8b-8192", but can be updated to use other models.
        /// </summary>
        public string model = "llama3-8b-8192";

        /// <summary>
        /// A custom permanent message that will be included with every API request.
        /// This can be used to set a context or introduction for the conversation.
        /// </summary>
        public string customPermanentMessage = "";

        #endregion
    }
}
