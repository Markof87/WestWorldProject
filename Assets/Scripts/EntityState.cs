using System.Collections;
using UnityEngine;

public abstract class EntityState
{
    public string stateName;

    public abstract void Enter(Miner miner);
    public abstract void Execute(Miner miner);
    public abstract void Exit(Miner miner);
}
