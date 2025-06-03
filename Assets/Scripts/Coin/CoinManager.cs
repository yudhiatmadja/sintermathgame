using UnityEngine;
using System.IO;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int totalCoins = 0;
    
    // Path untuk menyimpan data koin sebagai file
    private string saveFilePath;

    private void Awake()
    {
        // Tentukan path penyimpanan data
        saveFilePath = Path.Combine(Application.persistentDataPath, "coinData.json");
        Debug.Log("Save file path: " + saveFilePath);
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
            Debug.Log("CoinManager initialized with " + totalCoins + " coins");
        }
        else
        {
            Debug.Log("Destroying duplicate CoinManager");
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log("Added " + amount + " coins. Total now: " + totalCoins);
        SaveCoins();
    }

    public void SaveCoins()
    {
        // Menggunakan PlayerPrefs
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        
        // Backup menggunakan JSON file
        SaveToJson();
        
        Debug.Log("Coins saved: " + totalCoins + " (Both PlayerPrefs and JSON)");
    }

    public void LoadCoins()
    {
        // Coba load dari PlayerPrefs dulu
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        Debug.Log("Loaded " + totalCoins + " coins from PlayerPrefs");
        
        // Jika tidak ada di PlayerPrefs, coba dari file JSON
        if (totalCoins == 0)
        {
            LoadFromJson();
            Debug.Log("Loaded " + totalCoins + " coins from JSON backup");
        }
    }
    
    // Menyimpan data ke file JSON sebagai backup
    private void SaveToJson()
    {
        CoinData data = new CoinData { coins = totalCoins };
        string jsonData = JsonUtility.ToJson(data);
        
        try
        {
            File.WriteAllText(saveFilePath, jsonData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save coin data to JSON: " + e.Message);
        }
    }
    
    // Memuat data dari file JSON
    private void LoadFromJson()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
                totalCoins = data.coins;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load coin data from JSON: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        SaveCoins();
        Debug.Log("Application quitting, saving coins: " + totalCoins);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveCoins();
            Debug.Log("Application paused, saving coins: " + totalCoins);
        }
    }
    
    // Struktur data untuk JSON
    [System.Serializable]
    private class CoinData
    {
        public int coins;
    }
}