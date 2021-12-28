using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenState : IState
{
    private readonly RandomAccessMemory _ram;

    public ListenState(RandomAccessMemory ram)
    {
        _ram = ram;
    }

    public void Tick()
    {
        var phrase = _ram.TalkingTarget?.Api.Talking.CurrentPhrase;
        if (phrase == null)
        {
            return;
        }
        _ram.LastHeardPhrase = phrase;
    }

    public void OnEnter()
    {
        Tick();
    }

    public void OnExit()
    {
    }
}
