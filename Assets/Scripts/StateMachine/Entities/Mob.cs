using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CollidedItems _collided;

    public Tree AppleTree;
    public Shop AppleShop;

    private StateMachine _stateMachine;
    private RandomAccessMemory _ram = new RandomAccessMemory();

    public Vector2? TargetPosition// todo remove this
    {
        get
        {
            return _ram.TargetPosition;
        }
        set
        {
            _ram.TargetPosition = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _stateMachine = new StateMachine();

        var moveState = new MoveToTargetState(this, _rigidbody2D, _ram, _speed);
        var buyState = new BuyItemState(this, _wallet);
        var harvestState = new HarvestApplesState(this);
        var idleState = new IdleState();
        var thinkState = new ThinkState(this, _wallet);

        AT(thinkState, moveState, Transitions.TargetDetected(_ram));
        AT(moveState, harvestState, Transitions.CollidedObjectWithName(_collided, "Tree"));
        AT(moveState, buyState, Transitions.CollidedObjectWithName(_collided, "Shop"));


        _stateMachine.SetState(thinkState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }

}
