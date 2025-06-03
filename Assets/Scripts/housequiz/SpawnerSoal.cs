using UnityEngine;

public class SpawnerSoal : MonoBehaviour
{
    public GameObject soalPrefab;
    public float spawnInterval = 2f;
    [Tooltip("Margin agar soal tidak mepet banget ke tepi layar")]
    public float horizontalMargin = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnSoal), 1f, spawnInterval);
    }

    void SpawnSoal()
    {
        // Hitung batas X kamera
        Camera cam = Camera.main;
        float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 leftWorld  = cam.ViewportToWorldPoint(new Vector3(0, 0, zDist));
        Vector3 rightWorld = cam.ViewportToWorldPoint(new Vector3(1, 0, zDist));

        float minX = leftWorld.x  + horizontalMargin;
        float maxX = rightWorld.x - horizontalMargin;

        // Pilih posisi X random di antara batas
        float xPos = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(xPos, 6f, 0);

        // Instantiate
        GameObject soalObj = Instantiate(soalPrefab, spawnPos, Quaternion.identity);

        // Generate soal random (contoh + - x)
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int op = Random.Range(0, 3);

        string soal = "";
        int jawaban = 0;
        switch (op)
        {
            case 0: soal = $"{a} + {b}"; jawaban = a + b; break;
            case 1: soal = $"{a} - {b}"; jawaban = a - b; break;
            case 2: soal = $"{a} x {b}"; jawaban = a * b; break;
        }

        soalObj.GetComponent<SoalTextController>().SetSoal(soal, jawaban);
        GameManager.Instance.tambahSoal(soalObj);
    }
}
