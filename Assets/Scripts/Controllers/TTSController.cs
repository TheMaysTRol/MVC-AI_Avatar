using UnityEngine;
using UnityEngine.Events;
using static KJAvatar.ChatView;

namespace KJAvatar
{
    public class TTSController : MonoBehaviour
    {
        [SerializeField] private TTSView ttsView;
        [SerializeField] private TTSModel ttsModel;
        [SerializeField] private TTSService ttsService;

        public UnityEvent<AudioClip, string> onGetTTSFromApi;

        public async void GetTTS(Sender sender, string textToType)
        {
            var clip = await ttsService.SendMessageToAPI(textToType);
            onGetTTSFromApi?.Invoke(clip, textToType);
            ttsView.PlayAudio(clip);
        }

    }
}
