using UnityEngine;

public class AudioState : MonoBehaviour
{
    public static AudioState Instance { get; private set; }

    public bool isMusicMuted = false;
    public bool areSoundsMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}