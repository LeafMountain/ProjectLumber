using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storable : MonoBehaviour, IUnitInteractable
{
    public enum State
    {
        Normal = 0,
        InUse = 1,
        InInventory = 2,
    }

    public State state;
    public Inventory currentInventory;

    public IEnumerator PickUpInteraction(Unit unit)
    {
        yield return unit.navAgent.MoveToInteractable(GetComponent<Interactable>());
        currentInventory?.Withdraw(this);
        unit.inventory.Deposit(this);
    }

    public void Withdraw()
    {
        if(currentInventory)
        {
            currentInventory.Deposit(this);
        }
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        yield return PickUpInteraction(unit);
    }
}
