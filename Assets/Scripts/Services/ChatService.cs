using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// Service responsible for handling communication between the application and the API.
    /// This includes sending user messages to the API and receiving AI responses.
    /// </summary>
    public class ChatService : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private ChatModel model; /// The model holding API configuration (URL, key, etc.).

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends a user message to the API and retrieves the response.
        /// </summary>
        /// <param name="userMessage">The message sent by the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the AI response message.</returns>
        public async Task<string> SendMessageToAPI(List<ChatMessage> conversation)
        {
            // Ensure that the API configuration is available and valid
            if (model == null || string.IsNullOrEmpty(model.GetData().GetApiKey()))
            {
                Logger.Log("API configuration is missing or incomplete.", Logger.LogLevel.Error);
                return null;
            }

            // Retrieve API URL and key from the model
            string apiUrl = model.GetData().GetApiUrl();
            string apiKey = model.GetData().GetApiKey();


            // If a custom permanent message exists in the settings, add it to the message list
            if (!string.IsNullOrEmpty(model.GetData().GetApiCustomPermanentMessage()))
            {
                if (conversation.Count <= 1)
                {
                    conversation.Insert(0,new ChatMessage
                    {
                        role = "system",
                        content = model.GetData().GetApiCustomPermanentMessage()
                    });
                }
            }

            // Create the request payload
            ChatRequest requestPayload = new ChatRequest
            {
                model = "llama3-8b-8192",  /// Specifies the model to use for generating responses.
                messages = conversation        /// The list of messages to be sent to the API.
            };

            // Send the request and get the response
            var result = await APIConnections.ApiPostJsonAsync<ChatRequest, ChatResponse>(apiUrl, requestPayload, apiKey);

            // Return the content of the first choice from the response
            return result.choices[0].message.content;
        }

        #endregion
    }
}
