using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceNotification : MonoBehaviour
{
    public Image[] icons;
    public UIObjectTracker objectTracker;

    public void Set(ItemData[] items)
    {
        transform.DOScale(1f, .2f).SetEase(Ease.OutElastic);
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(i < items.Length);
            if (i < items.Length)
            {
                icons[i].sprite = items[i].icon;
            }
        }
    }
}
