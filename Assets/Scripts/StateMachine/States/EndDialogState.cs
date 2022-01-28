using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogState : IState
{
    private readonly RandomAccessMemory _ram;

    public EndDialogState(RandomAccessMemory ram, Animator animator)
    {
        _ram = ram;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _ram.TalkingTarget = null;
    }

    public void OnExit()
    {
    }
}
