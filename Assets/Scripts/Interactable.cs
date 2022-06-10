using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 2f;

    private bool isCollected = false;
    private ActionStateManager action;
    

    private void Start()
    {
        action = FindObjectOfType<ActionStateManager>();
    }
    private void Update()
    {
        if (Vector3.Distance(action.transform.position, transform.position) <= 2f && action.pickUpAction.triggered && !action.CanPickUp)
        {
            action.CanPickUp = true;
            isCollected = true;
        }

        if (isCollected) Destroy(gameObject);
    }
    private void OnDestroy()
    {
        action.CanPickUp = false;
    }
}
