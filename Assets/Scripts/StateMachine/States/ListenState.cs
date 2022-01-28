using UnityEngine;

public class ListenState : IState
{
    private readonly RandomAccessMemory _ram;
    private readonly ISpeechDecisionMaker _decisionMaker;
    private Coroutine _coroutine;

    public ListenState(RandomAccessMemory ram, ISpeechDecisionMaker decisionMaker)
    {
        _ram = ram;
        _decisionMaker = decisionMaker;
    }

    public void Tick()
    {
        MobExtensions.Listen( _ram );
    }

    public void OnEnter()
    {
        _coroutine = MobExtensions.StartGeneratingPhrase(_decisionMaker, _ram);
    }

    public void OnExit()
    {
        MobExtensions.StopGeneratingPhrase(_ram, _coroutine);
    }
}
