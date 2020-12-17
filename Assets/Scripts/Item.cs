using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static List<Item> items = new List<Item>();
    new public string name = "NO NAME ITEM";

    private void Awake()
    {
        items.Add(this);
    }

    private void OnDestroy()
    {
        items.Remove(this);
    }

    public static Item FindItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].GetType() == item.GetType())
            {
                if(items[i].GetComponent<Storable>().state != Storable.State.InUse)
                {
                    return items[i];
                }
            }
        }
        return null;
    }
}
