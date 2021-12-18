using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock
{
    public event Action Tick;

    private Coroutine _coroutine;
    private readonly MonoBehaviour _coroutineInvoker;

    public float IntervalSeconds { set; get; } = 1;

    public Clock(MonoBehaviour coroutineInvoker)
    {
        _coroutineInvoker = coroutineInvoker;
        //DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        _coroutine = _coroutineInvoker.StartCoroutine(ClockRoutine());
    }

    private IEnumerator ClockRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(IntervalSeconds);

            Tick?.Invoke();
        }
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            _coroutineInvoker.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
