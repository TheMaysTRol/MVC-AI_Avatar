using System.Collections.Generic;
using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// The model responsible for holding configuration data required for API communication.
    /// </summary>
    public class ChatModel : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private APIConfigModel settings; /// Configuration data for API, such as API keys and URL.

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the API configuration data.
        /// </summary>
        /// <returns>The API configuration model containing API settings.</returns>
        public APIConfigModel GetData() => settings;

        #endregion
    }

    #region Models for API Communication

    /// <summary>
    /// Model used for constructing a request to be sent to the API.
    /// </summary>
    [System.Serializable]
    public class ChatRequest
    {
        public string model;           /// The model identifier (e.g., "llama3-8b-8192").
        public List<ChatMessage> messages; /// The list of messages to be sent to the model.
    }

    /// <summary>
    /// Represents a single message, either from the user or the assistant.
    /// </summary>
    [System.Serializable]
    public class ChatMessage
    {
        public string role;    /// The role of the sender (e.g., "user", "assistant").
        public string content; /// The content of the message.
    }

    /// <summary>
    /// Model for receiving a response from the API.
    /// </summary>
    [System.Serializable]
    public class ChatResponse
    {
        public List<Choice> choices; /// A list of potential choices returned by the API.
    }

    /// <summary>
    /// Represents a choice made by the model in response to a given prompt.
    /// </summary>
    [System.Serializable]
    public class Choice
    {
        public ChatMessage message; /// The message of the choice.
    }

    #endregion
}
