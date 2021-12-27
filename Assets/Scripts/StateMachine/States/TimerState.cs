using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerState : IState, INamedState
{
    private readonly IState _state;
    private float _time;

    public TimerState(IState state)
    {
        _state = state;
    }

    public float TimeSinceEnter => Time.time - _time;

    public void Tick()
    {
        _state.Tick();
    }

    public void OnEnter()
    {
        _time = Time.time;
        _state.OnEnter();
    }

    public void OnExit()
    {
        _state.OnExit();
    }

    public string Name => _state.Name();
}

public static class TimerStateExtentions
{
    public static TimerState ToTimerState(this IState state)
    {
        return new TimerState(state);
    }

    public static bool IsTimeout(this TimerState state, float time)
    {
        return state.TimeSinceEnter >= time;
    }
}

