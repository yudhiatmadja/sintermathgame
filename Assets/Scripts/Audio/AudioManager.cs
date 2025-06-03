using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip hurtSound;
    public AudioClip jumpSound;
    public AudioClip enemyHurt;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "shoot":
                audioSource.PlayOneShot(shootSound);
                break;
            case "hurt":
                audioSource.PlayOneShot(hurtSound);
                break;
            case "jump":
                audioSource.PlayOneShot(jumpSound);
                break;
           

        }
    }
    
   
}
