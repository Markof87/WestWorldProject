/*
 * West World Project - GoHomeAndSleepTilRested (https://github.com/Markof87/BM-Project/blob/master/Assets/Scripts/Core/GameManager.cs)
 * 
 * In this state the miner should change location to be at home.
 * Once at home, if the miner is not fatigued EnterMineAndDigForNugget
 * otherwise keep sleeping until he's rested
 * 
 */

using System;
using System.Collections;
using UnityEngine;

public class GoHomeAndSleepTilRested : EntityState
{
    private static readonly Lazy<GoHomeAndSleepTilRested> lazy = new Lazy<GoHomeAndSleepTilRested>(() => new GoHomeAndSleepTilRested());

    public static GoHomeAndSleepTilRested Instance
    {
        get { return lazy.Value; }
    }

    public GoHomeAndSleepTilRested() {
        stateName = "Home";
    }

    public override void Enter(Miner miner)
    {
        //On entry the miner makes sure he is located at home
        if (miner.location != LocationType.SHACK)
        {
            Debug.Log(miner.ID + " Walkin home");
            miner.ChangeLocation(LocationType.SHACK);
        }
    }

    public override void Execute(Miner miner)
    {
        //If miner is not fatigued, start to dig for nuggets again
        if (!miner.IsFatigued())
        {
            Debug.Log(miner.ID + " What a God darn fantastic nap! Time to find more gold");
            miner.ChangeState(EnterMineAndDigForNugget.Instance);
        }
        //Else sleep
        else
        {
            miner.DecreaseFatigue();
            Debug.Log(miner.ID + " ZZZZZ...");
        }
    }

    public override void Exit(Miner miner)
    {
        Debug.Log(miner.ID + " Leaving the house");
    }
}
