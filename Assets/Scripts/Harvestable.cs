using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Harvestable : MonoBehaviour, IInteractable
{
    public Item item;
    public float harvestTime = 2f;

    public IEnumerator DoInteraction(Unit unit)
    {
        if (unit.inventory.IsFull())
        {
            yield return Inventory.StoreInteraction(unit);
            if (unit.inventory.IsFull())
            {
                yield break;
            }
        }

        yield return unit.navAgent.MoveToInteractableSeq(GetComponent<Interactable>());
        yield return unit.LookAt(GetComponent<Interactable>());
        float startTimestamp = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - startTimestamp < harvestTime)
        {
            unit.animator.SetTrigger("attackMelee");
            yield return new WaitForSeconds(.5f);
            transform.DOPunchRotation(new Vector3(10f, 0f, 0f), .3f);
            yield return new WaitForSeconds(.5f);
        }
        Storable spawned = Instantiate(item).GetComponent<Storable>();
        unit.inventory.Deposit(spawned);
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Icons/Placeholders/Hockey stick");
    }

    public string GetName()
    {
        return "Harvest";
    }

    public bool IsEnabled()
    {
        if (InteractionSystem.Instance.interactables.Count == 0 || InteractionSystem.Instance.interactables[0] == GetComponent<Interactable>())
        {
            return false;
        }
        return true;
    }
}
