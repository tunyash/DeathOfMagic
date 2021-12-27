using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTalkState : IState
{
    private readonly Rigidbody2D _rigidbody;
    private readonly RandomAccessMemory _ram;
    private readonly float _minDistance;
    private readonly float _maxDistance;
    private readonly float _speed;

    public MoveToTalkState(RandomAccessMemory ram, float minDistance, float maxDistance)
    {
        _ram = ram;
        _minDistance = minDistance;
        _maxDistance = maxDistance;
    }

    public void Tick()
    {
        var positionTo = _ram.TalkingTarget.Api.Position;

        var position = _rigidbody.position;
        var distance = (positionTo - position).magnitude;

        var direction = (positionTo - position).normalized;
        var speed = Mathf.Min(_speed * Time.deltaTime, distance);

        if (_maxDistance >= distance && distance >= _minDistance)
        {
            speed = 0;
        }
        else if (_minDistance > distance)
        {
            direction = direction * -1;
        }

        if (speed > 0)
        {
            _rigidbody.position = position + speed * direction;
        }
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}
