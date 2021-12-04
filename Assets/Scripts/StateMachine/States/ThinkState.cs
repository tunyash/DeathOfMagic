using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkState : IState
{
    private readonly Mob _mob;
    private readonly Wallet _wallet;

    public ThinkState(Mob mob, Wallet wallet)
    {
        _mob = mob;
        _wallet = wallet;
    }

    public void OnEnter()
    {
        if (_wallet.MoneyAmmount >= _mob.AppleShop.ApplePrice)
        {
            _mob.TargetPosition = _mob.AppleShop.GetComponent<Transform>().position;
            return;
        }

        _mob.TargetPosition = _mob.AppleTree.GetComponent<Transform>().position;
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public void Tick()
    {
        //throw new System.NotImplementedException();
    }
}
