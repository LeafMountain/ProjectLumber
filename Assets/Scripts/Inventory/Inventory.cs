using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInteractable
{
    public static List<Inventory> inventories = new List<Inventory>();
    public Storable[] storables;
    public Transform[] storableSlots;

    private void Awake()
    {
        inventories.Add(this);
    }

    private void OnDestroy()
    {
        inventories.Remove(this);
    }

    public bool Deposit(Storable storable)
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if (storables[i] == null)
            {
                storables[i] = storable;
                if (storableSlots.Length > i)
                {
                    storable.transform.SetParent(storableSlots[i]);
                    storable.transform.localPosition = Vector3.zero;
                    storable.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    storable.gameObject.SetActive(false);
                }
                storable.state = Storable.State.InInventory;
                storable.currentInventory = this;

                Item item = storable.GetComponent<Item>();
                if (GameManager.Instance.items.ContainsKey(item.data) == false)
                {
                    GameManager.Instance.items.Add(item.data, new List<Item>());
                }
                else
                {
                    GameManager.Instance.items[item.data].Add(item);
                }

                return true;
            }
        }
        return false;
    }

    public Storable Withdraw(Storable storable)
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if (storables[i] != null && storables[i] == storable)
            {
                storables[i] = null;
                storable.gameObject.SetActive(true);
                storable.gameObject.transform.SetParent(null);
                storable.state = Storable.State.Normal;
                storable.currentInventory = null;

                Item item = storable.GetComponent<Item>();
                if (GameManager.Instance.items.ContainsKey(item.data))
                {
                    GameManager.Instance.items[item.data].Remove(item);
                }

                return storable;
            }
        }
        return null;
    }

    public bool IsFull()
    {
        for (int i = 0; i < storables.Length; i++)
        {
            if (storables[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void Transfer(Inventory otherInventory)
    {
        if (IsFull())
        {
            return;
        }

        for (int i = 0; i < otherInventory.storables.Length; i++)
        {
            if (IsFull())
            {
                return;
            }
            if (otherInventory.storables[i] != null)
            {
                Storable storable = otherInventory.Withdraw(otherInventory.storables[i]);
                Deposit(storable);
            }
        }
    }

    public static IEnumerator StoreInteraction(Unit unit)
    {
        Inventory targetInventory = null;
        for (int i = 0; i < inventories.Count; i++)
        {
            if (inventories[i].IsFull() == false && Inventory.inventories[i].GetComponent<Unit>() == false && Inventory.inventories[i].GetComponent<Building>().state == Building.State.Normal)
            {
                yield return unit.navAgent.MoveToPositionSeq(Inventory.inventories[i].transform.position);
                if (inventories[i].IsFull())
                {
                    continue;
                }
                targetInventory = inventories[i];
                break;
            }
        }

        if (targetInventory == null)
        {
            yield return null;
            yield break;
        }

        yield return unit.navAgent.MoveToPositionSeq(targetInventory.transform.position);
        if (targetInventory.IsFull())
        {
            // Failed... Try again?
        }
        targetInventory.Transfer(unit.inventory);
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        if (GetComponent<Unit>() == false && unit.inventory.GetCount() > 0)
        {
            yield return StoreInteraction(unit);
        }
    }

    public int GetCount()
    {
        int count = 0;
        for (int i = 0; i < storableSlots.Length; i++)
        {
            if (storableSlots != null)
            {
                count++;
            }
        }
        return count;
    }

    public bool IsEnabled()
    {
        if(InteractionSystem.Instance.interactables.Count == 0 || InteractionSystem.Instance.interactables[0] == GetComponent<Interactable>())
        {
            return false;
        }

        Building building = GetComponent<Building>();
        if(building)
        {
            return building.state == Building.State.Normal;
        }
        return true;
    }

    public string GetName()
    {
        Building building = GetComponent<Building>();
        if(building)
        {
            return "Store";
        }
        else
        {
            return "Give";
        }
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Icons/Placeholders/Gift box");
    }
}
