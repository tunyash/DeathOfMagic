using System;
using System.Collections.Generic;
using UnityEngine;

// Notes
// 1. What a finite state machine is
// 2. Examples where you'd use one
//     AI, Animation, Game State
// 3. Parts of a State Machine
//     States & Transitions
// 4. States - 3 Parts
//     Tick - Why it's not Update()
//     OnEnter / OnExit (setup & cleanup)
// 5. Transitions
//     Separated from states so they can be re-used
//     Easy transitions from any state

public class StateMachine
{
    private readonly bool _shouldLog;
    private IState _currentState;
    private float _timeStart;

    // todo transitions by Name()..? not by type
    private Dictionary<string, List<Transition>> _transitions = new Dictionary<string, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private readonly static List<Transition> EmptyTransitions = new List<Transition>(0);

    public string CurrentStateName => _currentState.Name();

    public StateMachine( bool shouldLog = false )
    {
        _shouldLog = shouldLog;
        _timeStart = Time.realtimeSinceStartup;
    }

    public float StateTimeSeconds => Time.realtimeSinceStartup - _timeStart;

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }

        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if( _shouldLog )
        {
            Debug.Log($"SetState {state.Name()}");
        }

        if (state == _currentState)
        {
            return;
        }

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.Name(), out _currentTransitions);

        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }

        _currentState.OnEnter();

        _timeStart = Time.realtimeSinceStartup;
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if( _shouldLog )
        {
            Debug.Log( $"AddTransition from {from.GetType()}, to {to.GetType()}" );
        }

        if (_transitions.TryGetValue(from.Name(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.Name()] = transitions;
        }
      
        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
        {
            if (transition.Condition())
            {
                return transition; 
            }
        }

        foreach (var transition in _currentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }
}

public static class StateMachineExtentions
{
    public static string Name(this IState state)
    {
        return (state as INamedState)?.Name ?? state.GetType().Name;
    }
}
