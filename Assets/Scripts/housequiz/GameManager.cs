using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> soalAktif = new List<GameObject>();

    [Header("Effects")]
    public GameObject popEffectPrefab;  
    public float sfxVolume = 0.5f;      

    void Awake() => Instance = this;

    public void tambahSoal(GameObject obj)
    {
        soalAktif.Add(obj);
    }

public void CekJawaban(string input)
    {
        if (!int.TryParse(input, out int angka)) return;

        for (int i = 0; i < soalAktif.Count; i++)
        {
            var obj = soalAktif[i];
            var ctrl = obj.GetComponent<SoalTextController>();
            if (ctrl.jawaban == angka)
            {
                Instantiate(popEffectPrefab, obj.transform.position, Quaternion.identity);

            Audio.Instance.PlaySound("corect");
                ScoreManager.Instance.AddScore(10);

                Destroy(obj);
                soalAktif.RemoveAt(i);
                break;
            }
        }
    }
    public void RemoveSoal(GameObject obj)
    {
        if (soalAktif.Contains(obj))
            soalAktif.Remove(obj);
    }
}
