using UnityEngine;

namespace KJAvatar
{
    /// <summary>
    /// ScriptableObject that stores settings for a typewriter effect, including the delay between characters,
    /// the delay for punctuation marks, and the characters considered as punctuation.
    /// These settings are used by the TypewriterEffect to control the appearance of text in the UI.
    /// </summary>
    [CreateAssetMenu(fileName = "TypeWriter", menuName = "KJAvatar/TypeWriterSO", order = 2)]
    public class TypeWriterSO : ScriptableObject
    {
        #region Serialized Fields

        /// <summary>
        /// The delay (in seconds) between each character when typing the text.
        /// This controls the speed of the typewriter effect.
        /// </summary>
        [Header("Typing Settings")]
        public float letterDelay = 0.05f;

        /// <summary>
        /// The delay (in seconds) applied when a punctuation mark is encountered.
        /// Punctuation marks are displayed with a longer delay to create emphasis.
        /// </summary>
        public float punctuationDelay = 0.15f;

        /// <summary>
        /// A string containing the characters considered as punctuation marks.
        /// This is used to determine if the punctuation delay should be applied.
        /// Defaults to common punctuation marks, but can be modified as needed.
        /// </summary>
        public string punctuationMarks = ".,!?-:;";

        #endregion
    }
}
