using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Bullet")){
            Destroy(gameObject);
            Destroy(other.gameObject);    
            // AudioManager.instance.PlaySound("enemyHurt");
        }
    }
}
