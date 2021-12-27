using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : IState
{
    private readonly Animator _animator;
    private readonly TextMesh _mesh;
    private readonly SpeechDecisionMaker _speech;
    private readonly RandomAccessMemory _ram;

    private bool _isBye;

    public TalkState(Animator animator, TextMesh mesh, SpeechDecisionMaker speech, RandomAccessMemory ram)
    {
        _animator = animator;
        _mesh = mesh;
        _speech = speech;
        _ram = ram;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _mesh.gameObject.SetActive(true);
        _mesh.text = _speech.Analyze(out _isBye);

        if (_isBye)
        {
            _ram.TalkingTarget = null;
        }

        _animator.SetBool(ActionName, true);
    }

    public void OnExit()
    {
        _mesh.gameObject.SetActive(false);
        _animator.SetBool(ActionName, false);
    }

    private string ActionName => _isBye ? Constants.AnimationParams.Wave : Constants.AnimationParams.Talk;
}
