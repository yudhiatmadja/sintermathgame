using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Transform playerTransform;
    public float interactionDistance = 2f;
    public DialogManager dialogManager;

    void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            dialogManager.StartDialog();
        }
        else
        {
            dialogManager.EndDialog();
        }
    }
}
