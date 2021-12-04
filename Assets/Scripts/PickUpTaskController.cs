using UnityEngine;

public class PickUpTaskController : MonoBehaviour
{
    [SerializeField] private PickUpTask _pickUpTask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _pickUpTask.ToPickUp = collision.gameObject;
    }
}
