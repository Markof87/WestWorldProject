/*
 * West World Project - EnterMineAndDigForNugget (https://github.com/Markof87/BM-Project/blob/master/Assets/Scripts/Core/GameManager.cs)
 * 
 * In this state the miner should change location to be at the gold mine.
 * Once at the gold mine he should dig for gold until his pockets are full,
 * when he should change state to VisitBankAndDepositNugget.
 * If the miner gets thirsty while digging he should change state to
 * QuenchThirsty
 * 
 */

using System;
using UnityEngine;

public class EnterMineAndDigForNugget : EntityState
{
    private static readonly Lazy<EnterMineAndDigForNugget> lazy = new Lazy<EnterMineAndDigForNugget>(() => new EnterMineAndDigForNugget());

    public static EnterMineAndDigForNugget Instance
    {
        get { return lazy.Value; }
    }

    public EnterMineAndDigForNugget() {
        stateName = "Miner";
    }

    public override void Enter(Miner miner)
    {
        //If the miner is not already located at the gold mine, he must change location to the gold mine.
        if(miner.location != LocationType.GOLDMINE)
        {
            Debug.Log(miner.ID + " Walkin' to the gold mine");
            miner.ChangeLocation(LocationType.GOLDMINE);
        }
    }

    public override void Execute(Miner miner)
    {
        //The miner digs for gold until he's carrying in excess of MaxNuggets.
        //If he gets thirsty during his digging he stops work and changes state to go to the saloon for a whiskey.
        miner.AddToGoldCarrier(1);

        //Diggin' is hard work
        miner.IncreaseFatigue();
        Debug.Log(miner.ID + " Pickin' up a nugget");

        //If enough gold mined, go and put it in the bank
        if (miner.IsPocketFull())
            miner.ChangeState(VisitBankAndDepositGold.Instance);

        //If thirsty go and get a whiskey
        if (miner.IsThirsty())
            miner.ChangeState(QuenchThirst.Instance);
    }

    public override void Exit(Miner miner)
    {
        Debug.Log(miner.ID + " Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
    }
}
