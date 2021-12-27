using System;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public event Action<GameObject> EnterCollision;
    public event Action<GameObject> ExitCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterCollision?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitCollision?.Invoke(collision.gameObject);
    }
}
