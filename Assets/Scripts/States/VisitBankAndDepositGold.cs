/*
 * West World Project - VisitBankAndDepositGold (https://github.com/Markof87/BM-Project/blob/master/Assets/Scripts/Core/GameManager.cs)
 * 
 * In this state the miner should change location to be at the bank.
 * Once at the bank he should deposit on carry the gold he pick up.
 * If the miner gets the minimum comfort level gold, he can GoHomeAndSleepTilRested, 
 * otherwise he has to EnterMineAndDigForNugget again.
 * 
 */

using System;
using UnityEngine;

public class VisitBankAndDepositGold : EntityState
{
    private static readonly Lazy<VisitBankAndDepositGold> lazy = new Lazy<VisitBankAndDepositGold>(() => new VisitBankAndDepositGold());

    public static VisitBankAndDepositGold Instance
    {
        get { return lazy.Value; }
    }

    public VisitBankAndDepositGold() {
        stateName = "Bank";
    }

    public override void Enter(Miner miner)
    {
        //On entry the miner makes sure he is located at the bank
        if(miner.location != LocationType.BANK)
        {
            Debug.Log(miner.ID + " Goin' to the bank. Yes sireee!");
            miner.ChangeLocation(LocationType.BANK);
        }
    }

    public override void Execute(Miner miner)
    {
        //Deposit the gold
        miner.AddToWealth(miner.goldCarried);
        miner.goldCarried = 0;
        Debug.Log(miner.ID + " Depositing gold. Total savings now: " + miner.moneyInBank);

        //Wealthy enough to have a well earned rest?
        if (miner.moneyInBank >= miner.GetComfortLevel())
        {
            Debug.Log(miner.ID + " WooHoo! Rich enough for now. Back home to mah li'lle lady");
            miner.ChangeState(GoHomeAndSleepTilRested.Instance);
        }
        //Otherwise get more gold
        else
            miner.ChangeState(EnterMineAndDigForNugget.Instance);
    }

    public override void Exit(Miner miner)
    {
        Debug.Log(miner.ID + " Leavin' to the bank.");
    }
}
