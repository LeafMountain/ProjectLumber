using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour, IInteractable
{
    public string interactionText = "Work";
    public ItemData createsItem;
    public float productionTime = 10f;

    public Transform[] growthPosition;

    [Header("Read Only")]
    public float progressTime = -1;
    public Item[] growingItems;

    public float progressPercentage => progressTime / productionTime;

    private void Awake()
    {
        growingItems = new Item[growthPosition.Length];
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        if(Vector3.Distance(unit.transform.position, transform.position) > 1f)
        {
            yield return unit.navAgent.MoveToPositionSeq(transform.position);
            UI.ShowNotification(unit.interactable, $"Working on {GetComponent<Interactable>().GetName()}!");
        }

        // Harvest
        if(progressPercentage >= 1)
        {
            for (int i = 0; i < growingItems.Length; i++)
            {
                if(growingItems[i] != null)
                {
                    growingItems[i].GetComponent<Interactable>().enabled = true;
                    yield return growingItems[i].GetComponent<Storable>().PickUpInteraction(unit);
                    yield return Inventory.StoreInteraction(unit);
                }
            }
            progressTime = -1;
        }

        // Plant
        if(progressTime < 0)
        {
            yield return unit.navAgent.MoveToPositionSeq(transform.position);

            for (int i = 0; i < growingItems.Length; i++)
            {
                growingItems[i] = Instantiate(createsItem.prefab, growthPosition[i].position, growthPosition[i].rotation, growthPosition[i]).GetComponent<Item>();
                growingItems[i].GetComponent<Interactable>().enabled = false;
                growingItems[i].transform.localScale = new Vector3(1f, 0f, 1f);
            }
            progressTime = 0;
        }

        // Tend to crops
        while(progressPercentage < 1)
        {
            for (int i = 0; i < growingItems.Length; i++)
            {
                growingItems[i].transform.DOScaleY(progressPercentage, .2f).SetEase(Ease.OutBounce);
            }
            yield return new WaitForSeconds(1f);
            //progressTime += Time.deltaTime;
            progressTime += 1f;
            if(progressPercentage >= 1)
            {
                for (int i = 0; i < growingItems.Length; i++)
                {
                    growingItems[i].transform.DOPunchScale(Vector3.one, .2f);
                }
            }
        }
    }

    public Sprite GetIcon()
    {
        return null;
    }

    public string GetName()
    {
        return interactionText;
    }

    public bool IsEnabled()
    {
        return GetComponent<Building>().state == Building.State.Normal;
    }
}
