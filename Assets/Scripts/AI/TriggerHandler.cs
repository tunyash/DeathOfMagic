using System;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [ SerializeField ] private bool _shouldLog = false;

    public event Action<GameObject> EnterCollision;
    public event Action<GameObject> ExitCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( _shouldLog )
        {
            Debug.Log( $"{nameof(OnTriggerEnter2D)} {collision.name}" );
        }
        EnterCollision?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_shouldLog)
        {
            Debug.Log($"{nameof(OnTriggerExit2D)} {collision.name}");
        }
        ExitCollision?.Invoke(collision.gameObject);
    }
}
