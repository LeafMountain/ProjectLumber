using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Topbar : MonoBehaviour
{
    public GenericUIElement[] resourceFields;

    public ItemData[] trackItems;

    public void Update()
    {
        for (int i = 0; i < resourceFields.Length; i++)
        {
            resourceFields[i].gameObject.SetActive(i < trackItems.Length);
            if (i < trackItems.Length)
            {
                bool iconExits = trackItems[i].icon;
                resourceFields[i].icon.gameObject.SetActive(iconExits);
                resourceFields[i].icon.sprite = trackItems[i].icon;

                string text = string.Empty;
                if (iconExits == false)
                {
                    text += $"{trackItems[i].name}";
                }

                if (GameManager.Instance.items.ContainsKey(trackItems[i]))
                {
                    text += ($": {GameManager.Instance.items[trackItems[i]].Count.ToString()}");
                }
                else
                {
                    text += ($"{trackItems[i].name}: 0");
                }
                resourceFields[i].text.SetText($"{text}");

            }
        }
    }
}
