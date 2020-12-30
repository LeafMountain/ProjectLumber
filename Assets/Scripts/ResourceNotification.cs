using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceNotification : MonoBehaviour
{
    public ResourceBubble[] resourceBubbles;
    public UIObjectTracker objectTracker;

    public void Set(ItemData[] items)
    {
        transform.DOScale(1f, .2f).SetEase(Ease.OutElastic);
        for (int i = 0; i < resourceBubbles.Length; i++)
        {
            resourceBubbles[i].gameObject.SetActive(i < items.Length);
            if (i < items.Length)
            {
                resourceBubbles[i].icon.sprite= items[i].icon;
            }
        }
    }
}
