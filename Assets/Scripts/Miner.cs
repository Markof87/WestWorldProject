using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LocationType { SHACK, GOLDMINE, BANK, SALOON };

public class Miner : BaseGameEntity
{
    /* LLOCATIONS */
    [Header("Destinations")]
    [SerializeField]
    private GameObject homeDestination;

    [SerializeField]
    private GameObject saloonDestination;

    [SerializeField]
    private GameObject mineDestination;

    [SerializeField]
    private GameObject bankDestination;

    /* INITIALIZE FROM INSPECTOR */
    //The amount of gold a miner must have before he feels comfortable
    [SerializeField]
    private int comfortLevel = 5;

    //The amount of nuggets a miner can carry
    [SerializeField]
    private int maxNuggets = 5;

    //Above this value a miner is thirsty
    [SerializeField]
    private int thirstLevel = 5;

    //Above this value a miner is sleepy
    [SerializeField]
    private int tirednessThreshold = 5;

    /* INITIALIZE FROM START */
    private EntityState currentState;

    //The place where the miner is currently situated
    public LocationType location { get; set; }

    //How many nuggets the miner has in his pockets
    public int goldCarried { get; set; }

    //How much money the miner has deposited in the bank
    public int moneyInBank { get; set; }

    //The higher the value, the thirstier the miner
    public int thirst { get; set; }

    //The higher the value, the more tired the miner
    public int fatigue { get; set; }

    private NavMeshAgent agent;
    private Animator animator;

    private bool isChangeState = false;

    public GameObject exit;

    /* SERVICES METHODS */
    public int GetComfortLevel()
    {
        return comfortLevel;
    }

    public void ChangeLocation(LocationType locationType)
    {
        if (locationType != location)
        {
            location = locationType;

            switch (locationType)
            {
                case LocationType.BANK:
                    agent.SetDestination(bankDestination.transform.position);
                    break;

                case LocationType.GOLDMINE:
                    agent.SetDestination(mineDestination.transform.position);
                    break;

                case LocationType.SALOON:
                    agent.SetDestination(saloonDestination.transform.position);
                    break;

                case LocationType.SHACK:
                    agent.SetDestination(homeDestination.transform.position);
                    break;
            }
        }
    }

    public void AddToGoldCarrier(int val)
    {
        goldCarried += val;
        if (goldCarried < 0)
            goldCarried = 0;
    }

    public bool IsPocketFull()
    {
        return goldCarried >= maxNuggets;
    }

    public bool IsFatigued()
    {
        return fatigue >= tirednessThreshold;
    }

    public void IncreaseFatigue()
    {
        fatigue++;
    }

    public void DecreaseFatigue()
    {
        fatigue--;
    }

    public void AddToWealth(int val)
    {
        moneyInBank += val;
        if (moneyInBank < 0)
            moneyInBank = 0;
    }

    public bool IsThirsty()
    {
        Debug.Log("Current Thirst: " + thirst + " - Max Thirst: " + thirstLevel);
        return thirst >= thirstLevel;
    }

    public void IncreaseThirst()
    {
        thirst++;
    }

    public void DecreaseThirst()
    {
        thirst--;
    }

    public void BuyAndDrinkWhisky()
    {
        thirst = 0;
        moneyInBank -= 2;
    }

    public void ChangeState(EntityState newState)
    {
        isChangeState = true;

        //Call the exit method of the existing state
        transform.position = exit.transform.position;
        currentState.Exit(this);

        //Change state to the new state
        currentState = newState;

        //Call the entry method of the new state
        currentState.Enter(this);
    }

    private void UpdateMiner()
    {
        isChangeState = false;
        agent.ResetPath();
        IncreaseThirst();
        if (currentState != null)
            animator.SetTrigger(currentState.stateName);
    }

    public void ExecuteState()
    {
        currentState.Execute(this);
        if (!isChangeState)
            UpdateMiner();
    }

    private void StartMiner()
    {
        SetID(0);

        location = LocationType.SHACK;
        goldCarried = 0;
        moneyInBank = 0;
        thirst = 0;
        fatigue = 0;
        currentState = GoHomeAndSleepTilRested.Instance;
        transform.position = homeDestination.transform.position;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartMiner();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude != 0)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Destination" && other.CompareTag(currentState.stateName))
        {
            exit = other.gameObject;
            transform.position = other.transform.parent.Find("Action").position;
        }

        if (other.name == "Action")
            UpdateMiner();
    }
}
