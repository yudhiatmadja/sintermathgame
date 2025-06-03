using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    public Text coinText;
    
    void Start()
    {
        UpdateCoinDisplay();
        Debug.Log("CoinDisplay started, showing: " + (coinText != null ? coinText.text : "no text component"));
    }
    
    void Update()
    {
        UpdateCoinDisplay();
    }
    
    void UpdateCoinDisplay()
    {
        if (coinText != null && CoinManager.Instance != null)
        {
            coinText.text = CoinManager.Instance.totalCoins.ToString();
        }
        else if (coinText == null)
        {
            Debug.LogError("CoinDisplay: Text component is missing!");
        }
        else if (CoinManager.Instance == null)
        {
            Debug.LogWarning("CoinDisplay: CoinManager.Instance is null. Creating one...");
            // Auto-create CoinManager if missing
            GameObject coinManagerObj = new GameObject("CoinManager");
            coinManagerObj.AddComponent<CoinManager>();
        }
    }
}