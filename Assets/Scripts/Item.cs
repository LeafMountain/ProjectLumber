using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    public static Item FindItem(ItemData item)
    {
        if (GameManager.Instance.items.ContainsKey(item) == false)
        {
            return null;
        }

        List<Item> items = GameManager.Instance.items[item];
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].data == item)
                {
                    if (items[i].GetComponent<Storable>().state != Storable.State.InUse)
                    {
                        return items[i];
                    }
                }
            }
        }
        return null;
    }
}
