using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    private Vector3 lastCheckpoint = new Vector3(0, 2, 0); // Default posisi spawn awal

    private void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject); // Jangan hancurkan saat pindah scene
    }
    else
    {
        Destroy(gameObject);
    }
}


    public void SetCheckpoint(Vector3 position)
{
    lastCheckpoint = new Vector3(position.x, position.y, 0); // Pastikan Z tetap 0
    Debug.Log("Checkpoint disimpan di: " + lastCheckpoint);
}


public Vector3 GetCheckpoint()
{
    Vector3 safePosition = new Vector3(lastCheckpoint.x, lastCheckpoint.y, 0);
    Debug.Log("Player respawn di: " + safePosition);
    return safePosition;
}

}
