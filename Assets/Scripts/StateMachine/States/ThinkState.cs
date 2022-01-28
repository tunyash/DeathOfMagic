using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly Rect _room;
    private readonly Mob _mob;
    private readonly Wallet _wallet;
    private float _timeEnd;

    public ThinkState(RandomAccessMemory ram, Rect room)
    {
        _ram = ram;
        _room = room;
    }

    public void OnEnter()
    {
        _timeEnd = Time.realtimeSinceStartup + Random.Range(0, 3f);
        _ram.TargetPosition = null;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (Time.realtimeSinceStartup >= _timeEnd)
        {
            _ram.TargetPosition = new Vector2(Random.Range(_room.min.x, _room.max.x), Random.Range(_room.min.y, _room.max.y));
        }
    }
}
