using UnityEngine;

public class PickUpTask : MonoBehaviour
{
    public GameObject ToPickUp 
    {
        set
        {
            Debug.Log("ToPickUp");
            GameObject.Destroy(value);
            // put to inventory
        }
    }
}
