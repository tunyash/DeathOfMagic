using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogState : IState
{
    private readonly ISpeechDecisionMaker _decisionMaker;
    private readonly RandomAccessMemory _ram;

    private Coroutine _coroutine;

    public StartDialogState( ISpeechDecisionMaker decisionMaker, RandomAccessMemory ram )
    {
        _decisionMaker = decisionMaker;
        _ram = ram;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _ram.Dialog.Clear();

        _coroutine = MobExtensions.StartGeneratingPhrase( _decisionMaker, _ram );


        if (_ram.TalkingTarget.Api.Talking?.Target.Api.Talking.Target == null)
        {
            _ram.TalkingTarget.Api.Talking?.Target.Api.Talking.ForceTalk(_ram.ThisMob);
        }

    }

    public void OnExit()
    {
        MobExtensions.StopGeneratingPhrase(_ram, _coroutine);
    }
}
