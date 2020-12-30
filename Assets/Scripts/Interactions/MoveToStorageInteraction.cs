using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storable))]
public class MoveToStorageInteraction : MonoBehaviour, IInteractable
{
    Storable storable;

    private void Awake()
    {
        storable = GetComponent<Storable>();
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        if(storable.state != Storable.State.InWorld)
        {
            yield break;
        }

        yield return storable.DoInteraction(unit);
        yield return Inventory.StoreInteraction(unit);

        // Keep building the other buildings nearby
        Collider[] nearby = Physics.OverlapSphere(unit.transform.position, 10f);
        if (nearby != null)
        {
            for (int i = 0; i < nearby.Length; i++)
            {
                MoveToStorageInteraction nearbyStorable = nearby[i].GetComponent<MoveToStorageInteraction>();
                if (nearbyStorable && nearbyStorable.IsEnabled())
                {
                    yield return nearbyStorable.DoInteraction(unit);
                }
            }
        }
        yield break;
    }

    public Sprite GetIcon()
    {
        return null;
    }

    public string GetName()
    {
        return "Move to Storage";
    }

    public bool IsEnabled()
    {
        return storable.state == Storable.State.InWorld;
    }
}
