using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IMob
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;

    private StateMachine _stateMachine;
    private Transform _selectedFrame;
    private RandomAccessMemory _ram;

    public MobApi Api { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        _ram = new RandomAccessMemory(this, this);
        Api = new MobApi(_ram, _animator, _rigidbody);
        _stateMachine = new StateMachine();

        // states
        var moveState = new MoveToTargetState(_rigidbody, _speed, () => _ram.TargetPosition);
        var idleState = new IdleState();

        // subscribe
        AT(idleState, moveState, Transitions.TargetDetected(_ram));
        AT(moveState, idleState, Transitions.TargetPositionReached(_rigidbody, _ram));

        //DialogStateMachine.Add(_stateMachine, _ram, _animator, this, _rigidbody, new SpeechDecisionMaker(), _speed, idleState, idleState, moveState );

        _stateMachine.SetState(idleState);

        _selectedFrame = GameObject.Find("SelectedFrame").transform;
    }

    // Update is called once per frame
    void Update()
    {
        var position = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(position);
        var collidedCollider = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 5).collider;

        Debug.DrawRay(ray.origin,  ray.direction, Color.red );
        Transform collidedTransform = null;

        if (collidedCollider != null )// && collidedCollider.gameObject.name == "ClickBox")
        {
            collidedTransform = collidedCollider.gameObject.transform;
            _selectedFrame.gameObject.SetActive( true );
            _selectedFrame.position = collidedTransform.position;
        }
        else
        {
            _selectedFrame.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if( collidedTransform != null )
            {
                var mob = collidedTransform.transform.gameObject.GetComponent<IMob>();
                _ram.TalkingTarget = mob;
            }

            _ram.TargetPosition = Camera.main.ScreenToWorldPoint(position);
        }

        if( Input.GetKeyDown( KeyCode.Space ) )
        {
            GamePause.Switch(  );

        }
        
        _stateMachine.Tick();
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }

    private void AddDialogs(ISpeechDecisionMaker speechAnalyzer, RandomAccessMemory ram, Animator animator, IState enterState, IState exitState)
    {
        var startState = new StartDialogState(speechAnalyzer, ram);
        var endState = new EndDialogState(ram, animator);

        var talkState = new TalkState(animator, speechAnalyzer, ram).ToTimerState();
        var listenState = new ListenState(ram, speechAnalyzer);

        Func<bool> cameToTalk = () => ram.TalkingTarget != null;
        Func<bool> isTalking = () => ram.Dialog.CurrentSaidPhrase != null;
        Func<bool> talkingTargetIsInterested = () => ram.TalkingTarget?.Api.Talking.Target?.Equals(ram.ThisMob) ?? false;
        Func<bool> talkingTargetIsTalking = () => ram.TalkingTarget?.Api.Talking.CurrentPhrase != null;
        Func<bool> saidBye = () => ram.Dialog.CurrentSaidPhrase?.IsGoodbye ?? false;

        var exitTransition = Transitions.NotPredicate(talkingTargetIsInterested);

        AT( enterState, startState, cameToTalk);

        AT(startState, listenState, talkingTargetIsTalking);
        AT(startState, talkState, isTalking);
        AT(startState, endState, exitTransition);

        AT(talkState, listenState, talkingTargetIsTalking);
        AT(talkState, endState, exitTransition);
        AT(talkState, endState, saidBye);

        AT(listenState, talkState, isTalking);
        AT(listenState, endState, exitTransition);

        
        AT(endState, exitState, exitTransition);

    }
}
