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
    [SerializeField] private float _speedRun = 1f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private TriggerHandler _visionHandler;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMesh _dialogText;

    private StateMachine _stateMachine;
    //private readonly RandomAccessMemory _ram;
    private Vision _vision;
    private TriggerSet _triggers = new TriggerSet();
    private VisionAnalyzer _visionAnalyzer;

    public MobApi Api { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        var ram  = new RandomAccessMemory(this, this);
        Api = new MobApi(ram, _animator, _rigidbody2D);

        _vision = new Vision(_visionHandler);
        _visionAnalyzer = new VisionAnalyzer(_visionHandler, ram, 0.2f);
        var speechAnalyzer = SpeechDecisionMaker.Instance;

        _stateMachine = new StateMachine();

        // states
        var moveState = new MoveToTargetState( _rigidbody2D, _speed, () => ram.TargetPosition );
        var idleState = new IdleState();
        var thinkState = new ThinkState( ram, Room.Instance.Size );
        var runFromDangerState = new RunAwayState(ram, _rigidbody2D, _speedRun);

        // subscribe
        AT(thinkState, moveState, Transitions.TargetDetected(ram));
        AT(moveState, thinkState, Transitions.TargetPositionReached(_rigidbody2D, ram));

        //DialogStateMachine.Add(_stateMachine, _ram, _animator, this, _rigidbody2D, speechAnalyzer, _speed, thinkState, moveState, thinkState );

        _stateMachine.AddAnyTransition(runFromDangerState, Transitions.SeeDanger(ram));
        //AT(runFromDangerState, thinkState, Transitions.NotPredicate(Transitions.SeeDanger(_ram)));
        AT(runFromDangerState, thinkState, Transitions.TargetPositionReached(_rigidbody2D, ram) );

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
