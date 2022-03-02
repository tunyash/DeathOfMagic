using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Player : MonoBehaviour, IMob
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private DialogUiViewBase _dialogUi;
    [SerializeField] private TriggerHandler _triggerCoursor;

    private SpriteFlipper _flipper = new SpriteFlipper(); // todo
    private StateMachine _stateMachine;
    private Transform _selectedFrame;
    private RandomAccessMemory _ram;
    private Coursor _coursor;

    public MobApi Api { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        _coursor = new Coursor(_triggerCoursor);
        _ram = new RandomAccessMemory(this, this);
        Api = new MobApi(_ram, _animator, _rigidbody);
        _stateMachine = new StateMachine();
        _flipper.Parent = gameObject;

        // states
        var moveState = new MoveToTargetState( _animator, _rigidbody, _speed, () => _ram.TargetPosition, null, _flipper, _ram);
        var idleState = new IdleState();
        var speechAnalyzer = new PlayerSpeechDecisionMaker( _dialogUi as IPlayerDialogUiView );

        // subscribe
        AT(idleState, moveState, Transitions.TargetDetected(_ram));
        AT(moveState, idleState, Transitions.TargetPositionReached(_rigidbody, _ram));

        _stateMachine.AddDialogs(speechAnalyzer, _dialogUi, _rigidbody, _speed, _ram, _animator, idleState, moveState, idleState);

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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _coursor.Activated = true;
            _coursor.Position = Camera.main.ScreenToWorldPoint( position );
            if( _coursor.Selected != null )
            {
                _ram.TalkingTarget = _coursor.Selected;
            }

            //if( collidedTransform != null )
            //{
            //    var mob = collidedTransform.gameObject.GetComponent<IMob>();
            //    _ram.TalkingTarget = mob;
            //}

            _ram.TargetPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            _coursor.Activated = false;
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
}
