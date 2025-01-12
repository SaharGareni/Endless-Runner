using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f,1f)]
        public float volume;

        [Range(0.1f,3f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound sound in sounds)
        {
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;             
            }
        }
    }
}
