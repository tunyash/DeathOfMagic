using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedItems : MonoBehaviour
{
    public HashSet<GameObject> CollidedObjects = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollidedObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CollidedObjects.Remove(collision.gameObject);
    }
}
