using UnityEngine;
using TMPro;

public class SoalTextController : MonoBehaviour
{
    public string soal;
    public int jawaban;
    public TextMeshPro textMesh;
    public float fallSpeed = 2f;
    public float offscreenMargin = 0.5f;
    public GameObject correctVFX;

    void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponentInChildren<TextMeshPro>();
    }

    public void SetSoal(string s, int j)
    {
        soal = s;
        jawaban = j;
        textMesh.text = s;
    }

    void Update()
    {
        // Gerakkan ke bawah
        transform.position += Vector3.down * Time.deltaTime * fallSpeed;

        // Cek apakah sudah melewati batas bawah kamera
        Camera cam = Camera.main;
        float zDist = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 bottomWorld = cam.ViewportToWorldPoint(new Vector3(0, 0, zDist));

        if (transform.position.y < bottomWorld.y - offscreenMargin)
        {
            // Remove dari GameManager dan Destroy
            GameManager.Instance.RemoveSoal(gameObject);
            Destroy(gameObject);
        }
    }
    public void DestroyWithEffect()
{
    Instantiate(correctVFX, transform.position, Quaternion.identity);
    Destroy(gameObject);
}
}
