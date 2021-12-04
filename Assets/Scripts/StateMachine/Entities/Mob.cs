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

    public Vector2? TargetPosition;
    public Tree AppleTree;
    public Shop AppleShop;

    private StateMachine _stateMachine;

    // Start is called before the first frame update
    void Awake()
    {
        _stateMachine = new StateMachine();

        var moveState = new MoveToTargetState(this, _rigidbody2D, _speed);
        var buyState = new BuyItemState(this, _wallet);
        var harvestState = new HarvestApplesState(this);
        var idleState = new IdleState();
        var thinkState = new ThinkState(this, _wallet);

        //Func<bool> CanAffordApples() => () => _wallet.MoneyAmmount >= AppleShop.ApplePrice;
        Func<bool> TargetDetected() => () => TargetPosition != null;
        Func<bool> TargetReached() => () => TargetPosition != null &&
                                              Vector2.Distance(_rigidbody2D.position, TargetPosition.Value) < 1f;
        Func<bool> CollideTree() => () => _collided.CollidedObjects.Any(o => o.name == "Tree");
        Func<bool> CollideShop() => () => _collided.CollidedObjects.Any(o => o.name == "Shop");

        AT(thinkState, moveState, TargetDetected());
        AT(moveState, harvestState, CollideTree());
        AT(moveState, buyState, CollideShop());


        _stateMachine.SetState(thinkState);

        /*
        Func<bool> StuckForOverASecond() => () => moveToSelected.TimeStuck > 1f;
        Func<bool> ReachedResource() => () => Target != null &&
                                              Vector3.Distance(transform.position, Target.transform.position) < 1f;

        Func<bool> TargetIsDepletedAndICanCarryMore() => () => (Target == null || Target.IsDepleted) && !InventoryFull().Invoke();
        Func<bool> InventoryFull() => () => _gathered >= _maxCarried;
        Func<bool> ReachedStockpile() => () => StockPile != null &&
                                               Vector3.Distance(transform.position, StockPile.transform.position) < 1f;
                                               */
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    private Func<bool> AndPredicate(Func<bool> f1, Func<bool> f2)
    {
        return () => f1() && f2();
    }

    private void AT(IState from, IState to, Func<bool> condition)
    {
        _stateMachine.AddTransition(from, to, condition);
    }

}
