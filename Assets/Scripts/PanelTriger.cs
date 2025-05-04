using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    public string panelName; // Nama panel yang ingin ditampilkan saat trigger tersentuh

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PanelManager panelManager = FindAnyObjectByType<PanelManager>();
        if (panelManager != null)
        {
            panelManager.ShowPanel(panelName);
        }
        else
        {
            Debug.LogError("PanelManager tidak ditemukan! Pastikan ada di scene.");
        }

        if (CheckpointManager.instance != null)
        {
            CheckpointManager.instance.SetCheckpoint(transform.position);
        }
        else
        {
            Debug.LogError("CheckpointManager belum terinisialisasi!");
        }
    }
}

}
