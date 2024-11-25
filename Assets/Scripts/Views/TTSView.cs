using UnityEngine;

namespace KJAvatar
{
    public class TTSView : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayAudio(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Logger.Log("Failed to load audio clip.", Logger.LogLevel.Error);
            }
        }

    }
}
