using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : IState
{
    private readonly Animator _animator;
    private readonly TextMesh _mesh;
    private readonly SpeechDecisionMaker _speech;
    private readonly RandomAccessMemory _ram;

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
        var phrase = _speech.GenerateSpeech();
        _ram.CurrentPhrase = phrase;

        _mesh.gameObject.SetActive(true);
        _mesh.text = phrase.Text;
        _animator.SetBool(phrase.IsGoodbye ? Constants.AnimationParams.Wave : Constants.AnimationParams.Talk, true);
    }

    public void OnExit()
    {
        _ram.CurrentPhrase = null;
        _mesh.gameObject.SetActive(false);
        _animator.SetBool(Constants.AnimationParams.Talk, false);
        _animator.SetBool(Constants.AnimationParams.Wave, false);
    }
}
