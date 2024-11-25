using UnityEngine;

namespace KJAvatar
{
    public class TTSModel : MonoBehaviour
    {
        public TTSConfigSO ttsConfig;

        public TTSRequest CreateTTSRequest(string text)
        {
            TTSRequest requestData = new TTSRequest
            {
                text = text,
                voice_engine = ttsConfig.voiceEngine,
                voice = ttsConfig.voice,
                output_format = ttsConfig.outputFormat
            };
            return requestData;
        }

        [System.Serializable]
        public class TTSRequest
        {
            public string text;
            public string voice_engine;
            public string voice;
            public string output_format;
        }
    }
}
