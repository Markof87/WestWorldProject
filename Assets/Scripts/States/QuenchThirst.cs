/*
 * West World Project - QuenchThirst (https://github.com/Markof87/BM-Project/blob/master/Assets/Scripts/Core/GameManager.cs)
 * 
 * In this state the miner should change location to be at the saloon.
 * Once at the saloon, if the miner can only be thirsty, buy a whiskey and drink before
 * EnterMineAndDigForNugget again
 * 
 */

using System;
using UnityEngine;

public class QuenchThirst : EntityState
{
    private static readonly Lazy<QuenchThirst> lazy = new Lazy<QuenchThirst>(() => new QuenchThirst());

    public static QuenchThirst Instance
    {
        get { return lazy.Value; }
    }

    public QuenchThirst() {
        stateName = "Saloon";
    }

    public override void Enter(Miner miner)
    {
        //On entry the miner makes sure he is located at saloon
        if (miner.location != LocationType.SALOON)
        {
            Debug.Log(miner.ID + " Boy, ah sure is thusty! Walkin to the saloon");
            miner.ChangeLocation(LocationType.SALOON);
        }
    }

    public override void Execute(Miner miner)
    {
        //If miner is thirsty, buy a whiskey and drink it
        if (miner.IsThirsty())
        {
            miner.BuyAndDrinkWhisky();
            Debug.Log(miner.ID + " That's mighty fine sippin liquer");

            //And then go back to dig mine
            miner.ChangeState(EnterMineAndDigForNugget.Instance);
        }
        //Else WE HAVE A LOGICAL ERROR HERE!
        else
            Debug.Log("ERROR ERROR ERROR");
    }

    public override void Exit(Miner miner)
    {
        Debug.Log(miner.ID + " Leaving the saloon, feelin' good");
    }
}
