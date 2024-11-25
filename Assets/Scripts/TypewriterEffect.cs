using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KJAvatar
{
    /// <summary>
    /// A class to apply a typewriter effect to a TextMeshProUGUI component.
    /// It simulates typing out text character by character with adjustable speed and handling punctuation delays.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TypewriterEffect : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Data")]
        public TypeWriterSO data;  /// Data for configuring the typewriter effect, including delays and punctuation handling.

        #endregion

        #region Events

        [Header("Events")]
        public UnityEvent onTypewriterStart;        /// Event triggered when the typewriter effect starts.
        public UnityEvent onTypewriterComplete;     /// Event triggered when the typewriter effect is complete.
        public UnityEvent<char> onCharacterTyped;   /// Event triggered for each character typed.

        #endregion

        #region Private Fields

        private string fullText;                /// The full text to be typed out.
        private Coroutine typewriterCoroutine;   /// The coroutine responsible for typing the text.
        private bool isTyping;                  /// A flag to check if typing is in progress.
        private ScrollRect scrollRect;          /// Reference to the ScrollRect for auto-scrolling.

        #endregion

        #region Public Fields
        public TextMeshProUGUI textComponent;  /// The TextMeshProUGUI component to apply the typewriter effect.
        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();  /// Get the TextMeshProUGUI component.
            textComponent.text = "";                         /// Clear any existing text.
            isTyping = false;                                 /// Ensure typing state is false initially.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the typewriter effect with the text currently set in the TextMeshProUGUI component.
        /// </summary>
        public void StartTypewriter()
        {
            fullText = textComponent.text;  /// Get the current text in the TextMeshProUGUI.
            StartTypewriter(fullText);      /// Start typing this text.
        }

        /// <summary>
        /// Starts the typewriter effect with a new text.
        /// </summary>
        /// <param name="textToType">The new text to be typed out.</param>
        public void StartTypewriter(string textToType)
        {
            if (isTyping)  ///< If already typing, stop the current typing process.
            {
                StopTypewriter();
            }

            fullText = textToType;             /// Set the new text to type.
            typewriterCoroutine = StartCoroutine(TypeText());  /// Start the typewriter coroutine.
        }

        /// <summary>
        /// Immediately shows the full text and stops the typewriter effect.
        /// </summary>
        public void StopTypewriter()
        {
            if (typewriterCoroutine != null)
            {
                StopCoroutine(typewriterCoroutine);  /// Stop the current coroutine.
            }

            textComponent.text = fullText;  /// Display the full text immediately.
            isTyping = false;               /// Set typing state to false.
            onTypewriterComplete?.Invoke(); /// Invoke the completion event.
        }

        /// <summary>
        /// Clears the text and resets the typewriter effect.
        /// </summary>
        public void Clear()
        {
            StopTypewriter();  /// Stop any ongoing typing effect.
            fullText = "";      /// Clear the full text.
            textComponent.text = "";  /// Clear the displayed text.
        }

        /// <summary>
        /// Sets the ScrollRect to be used for scrolling the text.
        /// </summary>
        /// <param name="scrollRect">The ScrollRect to attach to the typewriter.</param>
        public void SetScrollRect(ScrollRect scrollRect)
        {
            this.scrollRect = scrollRect;
        }

        /// <summary>
        /// Scrolls to the bottom of the text, ensuring the most recent text is visible.
        /// </summary>
        public void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();  /// Forces the canvas to update to reflect the changes.
            scrollRect.verticalNormalizedPosition = 0f;  /// Scroll to the bottom of the ScrollRect.
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Coroutine that types out the full text character by character with a delay.
        /// </summary>
        /// <returns>Yield return for each character typed.</returns>
        private IEnumerator TypeText()
        {
            isTyping = true;  /// Set the typing flag to true.
            textComponent.text = "";  /// Start with an empty text field.
            onTypewriterStart?.Invoke();  /// Invoke the start event.

            WaitForSeconds normalDelay = new WaitForSeconds(data.letterDelay);  /// Normal delay between letters.
            WaitForSeconds punctuationDelay = new WaitForSeconds(this.data.punctuationDelay);  /// Delay for punctuation.

            int visibleCount = 0;  /// Counter for the number of visible characters.
            while (visibleCount < fullText.Length)
            {
                textComponent.maxVisibleCharacters = ++visibleCount;  /// Increase the visible character count.
                textComponent.text = fullText;  /// Update the text field.

                char currentChar = fullText[visibleCount - 1];  /// Get the current character.
                onCharacterTyped?.Invoke(currentChar);  /// Trigger the event for the current character.

                // Use longer delay for punctuation marks
                if (data.punctuationMarks.Contains(currentChar.ToString()))
                {
                    yield return punctuationDelay;  /// Wait for punctuation delay.
                }
                else
                {
                    yield return normalDelay;  /// Wait for normal delay.
                }
                ScrollToBottom();  /// Scroll the text to the bottom.
            }

            isTyping = false;  /// Set typing state to false once typing is complete.
            onTypewriterComplete?.Invoke();  /// Trigger the completion event.
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns whether the typewriter is currently typing.
        /// </summary>
        public bool IsTyping => isTyping;

        /// <summary>
        /// Gets or sets the typing speed (in seconds per character).
        /// </summary>
        public float TypingSpeed
        {
            get => data.letterDelay;
            set => data.letterDelay = Mathf.Max(0.001f, value);  /// Ensure a minimum typing speed.
        }

        #endregion
    }
}
