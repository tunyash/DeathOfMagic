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
    private RandomAccessMemory _ram = new RandomAccessMemory();
    private Vision _vision;
    private TriggerSet _triggers = new TriggerSet();
    private VisionAnalyzer _visionAnalyzer;

    [SerializeField] private bool _isInput;

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
        var runFromDangerState = new RunAwayState(_ram, _rigidbody2D, _speedRun);

        // subscribe
        AT(thinkState, moveState, Transitions.TargetDetected(_ram));

        if (!_isInput)
        {
            AT(moveState, thinkState, Transitions.TargetReached(_rigidbody2D, () => _ram.TargetPosition));
        }

        if (!_isInput)
        {
            DialogStateMachine.Add(_stateMachine, _ram, _animator, _dialogText, this, _rigidbody2D, speechAnalyzer, _speed, moveState, thinkState );
        }

        _stateMachine.AddAnyTransition(runFromDangerState, Transitions.SeeDanger(_ram));
        AT(runFromDangerState, thinkState, Transitions.NotPredicate(Transitions.SeeDanger(_ram)));

        // start
        _stateMachine.SetState(_isInput ? (IState) moveState : thinkState);
    }

    private void Update()
    {
        if (_isInput)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                var pos = Input.mousePosition;
                _ram.TargetPosition = Camera.main.ScreenToWorldPoint(pos);
                Debug.Log(_ram.TargetPosition);
            }
        }

        _stateMachine.Tick();
        _triggers.Refresh();
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }
}
