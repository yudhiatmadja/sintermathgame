using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 1;
    // public GameObject impactEffect; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah mengenai player
        if (collision.CompareTag("Player"))
        {
            // Damage player
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Efek ledakan
            // if (impactEffect != null)
            // {
            //     Instantiate(impactEffect, transform.position, Quaternion.identity);
            // }

            // Hancurkan fireball
            Destroy(gameObject);
        }
        // Cek jika mengenai terrain/obstacle
        else if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        {
            // Efek ledakan
            // if (impactEffect != null)
            // {
            //     Instantiate(impactEffect, transform.position, Quaternion.identity);
            // }

            // Hancurkan fireball
            Destroy(gameObject);
        }
    }
}