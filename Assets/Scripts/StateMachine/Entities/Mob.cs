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
    [SerializeField] private DialogUiViewBase _dialogUi;
    [SerializeField] private bool _shouldLog = false;
    [SerializeField] private string _stateName = "oiuy";

    private StateMachine _stateMachine;
    //private readonly RandomAccessMemory _ram;
    private Vision _vision;
    private VisionAnalyzer _visionAnalyzer;

    public MobApi Api { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        var ram  = new RandomAccessMemory(this, this);
        Api = new MobApi(ram, _animator, _rigidbody2D);

        _vision = new Vision(_visionHandler);
        _visionAnalyzer = new VisionAnalyzer(_visionHandler, ram, 0.2f);
        var speechAnalyzer = new SpeechDecisionMaker(this);

        _stateMachine = new StateMachine(_shouldLog);

        // states
        var moveState = new MoveToTargetState( _rigidbody2D, _speed, () => ram.TargetPosition );
        var idleState = new ThinkState( ram, Room.Instance.Size );
        var runFromDangerState = new RunAwayState(ram, _rigidbody2D, _speedRun);

        // subscribe
        AT(idleState, moveState, Transitions.TargetDetected(ram));
        AT(moveState, idleState, Transitions.TargetPositionReached(_rigidbody2D, ram));

        _stateMachine.AddDialogs( speechAnalyzer, _dialogUi, _rigidbody2D, _speed, ram, _animator, idleState, moveState, idleState );

        _stateMachine.AddAnyTransition(runFromDangerState, Transitions.SeeDanger(ram));
        AT(runFromDangerState, idleState, Transitions.TargetPositionReached(_rigidbody2D, ram) );

        // start
        _stateMachine.SetState(idleState);
    }

    private void Update()
    {
        _stateMachine.Tick();
        _stateName = _stateMachine.CurrentStateName;
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }
}
