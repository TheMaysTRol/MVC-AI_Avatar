using System.Threading.Tasks;
using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Service responsible for handling communication between the application and the API.
    /// This includes sending user messages to the API and receiving AI responses.
    /// </summary>
    public class TTSService : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private TTSModel model; /// The model holding API configuration (URL, key, etc.).

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends a user message to the API and retrieves the response.
        /// </summary>
        /// <param name="userMessage">The message sent by the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the AI response message.</returns>
        public async Task<AudioClip> SendMessageToAPI(string textToType)
        {
            // Ensure that the API configuration is available and valid
            if (model == null || string.IsNullOrEmpty(model.ttsConfig.apiKey))
            {
                Logger.Log("API configuration is missing or incomplete.", Logger.LogLevel.Error);
                return null;
            }

            // Retrieve API URL and key from the model
            string apiUrl = model.ttsConfig.apiUrl;
            string apiKey = model.ttsConfig.apiKey;
            string userId = model.ttsConfig.userId;

            // Create the request payload
            var request = model.CreateTTSRequest(textToType);
            AudioClip clip = await APIConnections.GetAudioAsync<TTSModel.TTSRequest>(apiUrl, request, apiKey, userId);

            return clip;
        }

        #endregion
    }
}
