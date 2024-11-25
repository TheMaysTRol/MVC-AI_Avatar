using UnityEngine;

namespace KJAvatar
{
    [CreateAssetMenu(fileName = "TTSConfig", menuName = "KJAvatar/TTSConfig", order = 1)]
    public class TTSConfigSO : ScriptableObject
    {
        [Header("API Configuration")]
        public string apiUrl = "<YOUR_API_URL>";
        public string userId = "<YOUR_USER_ID>";
        public string apiKey = "<YOUR_API_KEY>";
        public string voice = "<YOUR_VOICE_MODEL>";

        [Header("TTS Settings")]
        public string voiceEngine = "Play3.0";
        public string outputFormat = "mp3";
    }
}
