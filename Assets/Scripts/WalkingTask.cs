using UnityEngine;

public class WalkingTask : MonoBehaviour
{
    [SerializeField] private float _delta = 0.01f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Rigidbody2D _rigidbody;

    public Vector2 PositionTo { set; private get; }

    public bool IsFinished 
    { 
        get
        {
            return Vector3.Distance(_rigidbody.position, PositionTo) <= _delta;
        }
    }

    private void Update()
    {
        var position = _rigidbody.position;
        var distance = (PositionTo - position).magnitude;
        var direction = (PositionTo - position).normalized;
        var speed = Mathf.Min(_speed * Time.deltaTime, distance);

        _rigidbody.position = position + speed * direction;

    }
}
