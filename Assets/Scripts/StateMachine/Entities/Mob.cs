using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IMob
{
    MobApi Api { get; }
}

public class Mob : MonoBehaviour, IMob
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private TriggerHandler _visionHandler;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMesh _dialogText;

    private StateMachine _stateMachine;
    private RandomAccessMemory _ram = new RandomAccessMemory();
    private Vision _vision;
    private TriggerSet _triggers = new TriggerSet();
    private VisionAnalyzer _visionAnalyzer;

    public MobApi Api { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Api = new MobApi(_ram, _animator, _rigidbody2D);

        _vision = new Vision(_visionHandler);
        _visionAnalyzer = new VisionAnalyzer(_visionHandler, _ram, 0.2f);
        var speechAnalyzer = new SpeechDecisionMaker(_ram);

        _stateMachine = new StateMachine();

        // states
        var moveState = new MoveToTargetState( _rigidbody2D, _speed, () => _ram.TargetPosition );
        var idleState = new IdleState();
        var thinkState = new ThinkState( _ram, Room.Instance.Size );

        // subscribe
        AT(thinkState, moveState, Transitions.TargetDetected(_ram));
        AT(moveState, thinkState, Transitions.TargetReached(_rigidbody2D, () => _ram.TargetPosition));

        // dialog
        const float talkingDistance = 2f;
        var waveState = new WaveState(this, _animator, _vision, _ram).ToTimerState();
        var walkToTalkState = new MoveToTargetState( _rigidbody2D, _speed, () => _ram.TalkingTarget?.Api?.Position, "talk" );
        var talkState = new TalkState(_animator, _dialogText, speechAnalyzer, _ram).ToTimerState();
        var listenState = new ListenState();

        Func<bool> hasTalkingTargetTransition = () => _ram.TalkingTarget != null;
        Func<bool> wavedBackTransition = () => _ram.TalkingTarget?.Api.TalkingTarget?.Equals(this) ?? false;
        Func<bool> waveFinishedTransition = () => waveState.IsTimeout(2);
        Func<bool> gotCloseToTalkingTarget = () => (_ram.TalkingTarget.Api.Position - _rigidbody2D.position).sqrMagnitude <= talkingDistance;
        Func<bool> talkingTargetIsTalking = () => _ram.TalkingTarget?.Api?.AnimationState == Constants.AnimationParams.Talk;
        Func<bool> talkingFinished = () => talkState.IsTimeout(1.5f);



        AT(moveState, waveState, hasTalkingTargetTransition);
        AT(waveState, thinkState, Transitions.NotPredicate(hasTalkingTargetTransition));
        AT(waveState, thinkState, Transitions.AndPredicate(waveFinishedTransition, Transitions.NotPredicate(wavedBackTransition)));
        AT(waveState, walkToTalkState, Transitions.AndPredicate(waveFinishedTransition, wavedBackTransition));
        AT(walkToTalkState, listenState, Transitions.AndPredicate(gotCloseToTalkingTarget, talkingTargetIsTalking));
        AT(walkToTalkState, talkState, Transitions.AndPredicate(gotCloseToTalkingTarget, Transitions.NotPredicate(talkingTargetIsTalking)));

        AT(talkState, listenState, Transitions.AndPredicate(talkingFinished, hasTalkingTargetTransition ));
        AT(talkState, thinkState, Transitions.AndPredicate(talkingFinished, Transitions.NotPredicate(hasTalkingTargetTransition)));

        AT(listenState, talkState, Transitions.NotPredicate(talkingTargetIsTalking));

        // start
        _stateMachine.SetState(thinkState);
    }

    private void Update()
    {
        _stateMachine.Tick();
        _triggers.Refresh();
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }

}
