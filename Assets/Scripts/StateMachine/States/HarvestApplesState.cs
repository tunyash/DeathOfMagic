
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestApplesState : IState
{
    private readonly Mob _mob;
    private readonly Rigidbody2D _rigidbody;
    private readonly float _speed;
    [SerializeField] private float _intervalSeconds = 1f;

    private float _timeStart;

    public HarvestApplesState(RandomAccessMemory ram)
    {
        _mob = mob;
    }

    public void OnEnter()
    {
        _timeStart = 0;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_mob.Ram.AppleTree.Apples.Count == 0)
        {
            return;
        }

        _timeStart += Time.deltaTime;
        if (_timeStart >= _intervalSeconds)
        {
            _timeStart = 0;

            var apple = _mob.AppleTree.Apples.Pop();
            GameObject.Destroy(apple);
        }

    }
}

    */
