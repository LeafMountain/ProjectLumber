using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Storable[] storables;
    
    public bool Deposit(Storable storable)
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if(storables[i] == null)
            {
                storables[i] = storable;
                storable.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    public Storable Withdraw(Storable storable)
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if(storables[i] == null)
            {
                storables[i] = null;
                storable.gameObject.SetActive(true);
                return storable;
            }
        }
        return null;
    }

    public bool IsFull()
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if(storables[i] == null)
            {
                return false;
            }
        }
        return true;
    }
}
