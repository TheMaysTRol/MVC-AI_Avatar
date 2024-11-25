
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KJAvatar
{
    /// <summary>
    /// Handles the display and interaction of the chat UI.
    /// This includes receiving user input, displaying messages, 
    /// and managing the message history and typing animation effects.
    /// </summary>
    public class ChatView : MonoBehaviour
    {
        #region Enums and Structs

        /// <summary>
        /// Enum to represent the sender of a message.
        /// </summary>
        public enum Sender
        {
            /// <summary>Represents the user sending the message.</summary>
            You,

            /// <summary>Represents the AI sending the message.</summary>
            Ai
        }

        /// <summary>
        /// Struct to store a message and its sender for message history management.
        /// </summary>
        public struct senderMessageStruct
        {
            public Sender sender;  ///< The sender of the message.
            public string message;  ///< The content of the message.
        }

        #endregion

        #region Serialized Fields

        [SerializeField] private TMP_InputField messageInputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private Button clearConversationBtn;
        [SerializeField] private TypewriterEffect messagePrefab;
        [SerializeField] private Transform messagesContent;
        [SerializeField] private int messagesHistoryCount = 20;
        [SerializeField] private bool useTypeWriterEffect = true;
        [SerializeField] private TypeWriterSO typeWriterData;
        [SerializeField] private ScrollRect messagesScrollRect;
        [SerializeField] private TTSController ttsController;

        #endregion

        #region Private Variables

        private Stack<senderMessageStruct> messages = new Stack<senderMessageStruct>();
        private bool isThereMessageDisplaying = false;
        private List<TypewriterEffect> texts = new List<TypewriterEffect>();
        private int currentMessage = 0;

        #endregion

        #region Unity Events

        public UnityEvent<Sender, string> OnUserMessageSubmitted;
        public UnityEvent<Sender, string> OnAiMessageSubmitted;
        public UnityEvent<Sender> OnAiMessageCompleted;
        public UnityEvent OnClearConversation;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Initializes the component by adding listeners to the send button and preparing message history.
        /// </summary>
        private void OnEnable()
        {
            sendButton.onClick.AddListener(SubmitMessage);
            clearConversationBtn.onClick.AddListener(ClearConversation);
        }

        /// <summary>
        /// Removes listeners from the send button when the component is disabled.
        /// </summary>
        private void OnDisable()
        {
            sendButton.onClick.RemoveListener(SubmitMessage);
        }

        /// <summary>
        /// Initializes the message display by creating the required number of message prefabs.
        /// </summary>
        private void Start()
        {
            PrepareMessages();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prepares the initial message objects for display based on the configured message history count.
        /// </summary>
        private void PrepareMessages()
        {
            for (int i = 0; i < messagesHistoryCount; i++)
            {
                TypewriterEffect text = Instantiate(messagePrefab, messagesContent);
                text.gameObject.SetActive(false);
                if (useTypeWriterEffect && typeWriterData != null)
                {
                    text.SetScrollRect(messagesScrollRect);
                    text.data = typeWriterData;
                }
                texts.Add(text);
            }
        }

        /// <summary>
        /// Adds a new message to the display queue or shows it immediately if no message is currently displaying.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The content of the message.</param>
        public void AddMessage(Sender sender, string message)
        {
            if (!isThereMessageDisplaying)
            {
                if (sender == Sender.You)
                {
                    DisplayMessage(sender, message);
                }
            }
            else
            {
                // Add the message to the queue for later display
                messages.Push(new senderMessageStruct { sender = sender, message = message });
            }
        }

        /// <summary>
        /// Displays a message directly in the UI, either immediately or after animation.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The content of the message.</param>
        public void DisplayMessage(Sender sender, string message)
        {
            if (sender == Sender.Ai)
            {
                if (ttsController)
                {
                    ttsController.onGetTTSFromApi.AddListener(((clip, message) =>
                    {
                        SendMessage(sender, message);
                        OnAiMessageSubmitted?.Invoke(sender, message);
                        ttsController.onGetTTSFromApi.RemoveAllListeners();
                    }));
                    ttsController.GetTTS(sender, message);
                }
                else
                {
                    OnAiMessageSubmitted?.Invoke(sender, message);
                    SendMessage(sender, message);
                }

            }
            else
            {
                SendMessage(sender, message);
            }
        }

        private void SendMessage(Sender sender, string message)
        {
            isThereMessageDisplaying = true;

            // Set the message text and display it
            texts[currentMessage].textComponent.text = $"{sender}: {message}\n";
            texts[currentMessage].transform.SetAsLastSibling();
            texts[currentMessage].gameObject.SetActive(true);

            // If typewriter effect is enabled, start the typing animation
            if (useTypeWriterEffect)
            {
                texts[currentMessage].StartTypewriter();
                texts[currentMessage].onTypewriterComplete.AddListener(() => { OnCompleteTypingMessage(sender,texts[currentMessage]); });
            }

            currentMessage++;
        }

        /// <summary>
        /// Called when typing is complete to process the next message in the queue.
        /// </summary>
        /// <param name="writer">The TypewriterEffect component that finished typing.</param>
        private void OnCompleteTypingMessage(Sender sender,TypewriterEffect writer)
        {
            writer.onTypewriterComplete.RemoveAllListeners();
            isThereMessageDisplaying = false;
            OnAiMessageCompleted?.Invoke(sender);

            // Display the next queued message if available
            if (messages.Count > 0)
            {
                var messageStruct = messages.Pop();
                StartCoroutine(DelayDisplayMessage(messageStruct.sender, messageStruct.message, 0.5f));
            }
        }

        private IEnumerator DelayDisplayMessage(Sender sender, string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            DisplayMessage(sender, message);
        }

        #endregion

        #region UI Methods

        /// <summary>
        /// Called by the UI Button on Send. Submits the message entered by the user.
        /// </summary>
        public void SubmitMessage()
        {
            if (!string.IsNullOrWhiteSpace(messageInputField.text))
            {
                // Trigger user message submission event
                OnUserMessageSubmitted?.Invoke(Sender.You, messageInputField.text);
                messageInputField.text = string.Empty;
            }
        }


        /// <summary>
        /// Called by the UI Button on clear. Clears the conversation from messages.
        /// </summary>
        public void ClearConversation()
        {
            messageInputField.text = string.Empty;
            foreach (var message in texts)
            {
                message.gameObject.SetActive(false);
            }
            OnClearConversation?.Invoke();
        }

        #endregion
    }
}
