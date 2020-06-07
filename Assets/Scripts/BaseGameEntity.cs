using UnityEngine;

public class BaseGameEntity : MonoBehaviour
{
    //Every entity has a unique identifying number
    private int id;

    //This is the next valid ID. Each time a BaseGameEntity is instantiated, this value is updated
    static int iNextValidID = 0;

    //This is called within the constructor to make sure the ID is set correctly.
    //It verifies that the value passed to the method is greater or equal to the next valid ID,
    //before setting the ID and incrementing the next valid ID.
    public void SetID(int val) {

        //Make sure the val is equal to or greater than the next available ID
        if(val > iNextValidID)
        {
            Debug.LogError("INVALID ID");
            return;
        }

        id = val;
        iNextValidID = id + 1;
    }

    public int ID{
        get { return id; }
    }
}
