using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public AudioClip fallSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && fallSound != null)
            {
                audioSource.PlayOneShot(fallSound);
            }
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                RespawnPlayer(other.gameObject);
            }
        }
    }

    void RespawnPlayer(GameObject player)
{
    Vector3 respawnPosition = CheckpointManager.instance.GetCheckpoint();

    // Pastikan Z = 0 agar tidak jatuh ke void
    player.transform.position = new Vector3(respawnPosition.x, respawnPosition.y, 0);

    Debug.Log("Player setelah respawn di: " + player.transform.position);
    
    StartCoroutine(BlinkEffect(player));
}




    IEnumerator BlinkEffect(GameObject player)
    {
        SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
        if (sprite == null) yield break;

        float blinkDuration = 2f; // Waktu berkedip (2 detik)
        float blinkSpeed = 0.2f;  // Kecepatan kedip
        float timer = 0f;

        while (timer < blinkDuration)
        {
            sprite.enabled = !sprite.enabled; // Hidup-matikan sprite
            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;
        }

        sprite.enabled = true; // Pastikan sprite terlihat setelah selesai

    }


}
