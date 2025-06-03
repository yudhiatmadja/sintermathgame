using UnityEngine;

public class LiftController : MonoBehaviour
{
    public Transform atasPos;
    public Transform bawahPos;
    public float speed = 10f;

    private Vector3 targetPos;
    private bool isMoving = false;

    void Start()
    {
        targetPos = transform.position; // posisi awal
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                isMoving = false;
        }
    }

    public void MoveUp()
    {
        targetPos = atasPos.position;
        isMoving = true;
    }

    public void MoveDown()
    {
        targetPos = bawahPos.position;
        isMoving = true;
    }
}
