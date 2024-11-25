using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KJAvatar
{
    /// <summary>
    /// The controller responsible for handling user interactions, 
    /// sending messages to the service, and updating the chat view with the responses.
    /// </summary>
    public class ChatController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private ChatView chatView;        /// Reference to the ChatView to interact with the UI.
        [SerializeField] private ChatModel apiModel;       /// Reference to the ChatModel for fetching API configurations.
        [SerializeField] private ChatService chatService;  /// Reference to the ChatService for sending messages to the API.
        private List<ChatMessage> conversation = new List<ChatMessage>(); // list of all messages
        #endregion

        #region Public Events

        /// <summary>
        /// Event triggered when a message is received from the API, passed to the view.
        /// </summary>
        public UnityEvent<ChatView.Sender, string> OnMessageRecievedFromApi;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Adds listener to handle user message submission when the component is enabled.
        /// </summary>
        private void OnEnable()
        {
            chatView.OnUserMessageSubmitted.AddListener(HandleMessageSubmission);
        }

        /// <summary>
        /// Removes the listener when the component is disabled to avoid memory leaks.
        /// </summary>
        private void OnDisable()
        {
            chatView.OnUserMessageSubmitted.RemoveListener(HandleMessageSubmission);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the submission of a user's message and triggers the AI response.
        /// This method is asynchronous as it requires fetching data from the API.
        /// </summary>
        /// <param name="sender">The sender of the message (You).</param>
        /// <param name="userMessage">The content of the message sent by the user.</param>
        private async void HandleMessageSubmission(ChatView.Sender sender, string userMessage)
        {
            // Display the user message in the chat view
            chatView.AddMessage(sender, userMessage);

            //add messages to conversation
            conversation.Add(new ChatMessage() { role = "user", content = userMessage });

            // Fetch AI response from the chat service
            var aiResponse = await chatService.SendMessageToAPI(conversation);

            //add messages to conversation
            conversation.Add(new ChatMessage() { role = "assistant", content = aiResponse });

            // Display the AI response in the chat view
            chatView.AddMessage(ChatView.Sender.Ai, aiResponse);

            // Trigger the event indicating that a message was received from the API
            OnMessageRecievedFromApi?.Invoke(ChatView.Sender.Ai, aiResponse);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Called by the UI Button on clear. Clears the conversation from messages.
        /// </summary>
        public void ClearConversation()
        {
            conversation.Clear();
        }

        #endregion
    }
}
