using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1f; // durasi hidup VFX sebelum dihancurkan

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
