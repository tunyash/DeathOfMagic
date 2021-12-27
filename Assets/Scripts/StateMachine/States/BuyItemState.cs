/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemState : IState
{
    private readonly Mob _mob;
    private readonly Wallet _wallet;

    public BuyItemState(Mob mob, Wallet wallet)
    {
        _mob = mob;
        _wallet = wallet;
    }

    public void OnEnter()
    {
        var affordable = Mathf.Min (_mob.AppleShop.Apples.Count, (int)Mathf.Floor(_wallet.MoneyAmmount / _mob.AppleShop.ApplePrice));

        _wallet.MoneyAmmount -= _mob.AppleShop.ApplePrice * affordable;

        for (var i = 0; i < affordable; i++)
        {
            var apple = _mob.AppleShop.Apples.Pop();
            GameObject.Destroy(apple);
        }
    }

    public void OnExit()
    {
    }

    public void Tick()
    {

    }
}
*/