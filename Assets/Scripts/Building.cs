using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Building : MonoBehaviour, IInteractable
{
    public enum State
    {
        Normal = 0,
        Building = 1,
        Placing = 2,
    }

    public BuildingData data;
    public State state;
    public NavMeshObstacle navObstacle;

    public List<Item> buildMaterials = new List<Item>();

    public ResourceNotification resourceNotification;

    private void OnDestroy()
    {
        if (resourceNotification)
        {
            Destroy(resourceNotification.gameObject);
        }
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.Normal:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                if (navObstacle)
                {
                    navObstacle.enabled = true;
                }
                break;
            case State.Building:
                {
                    if (resourceNotification == null)
                    {
                        resourceNotification = UI.GetResourceNotification();
                    }
                    resourceNotification.Set(GetMissingIngredients());
                    resourceNotification.objectTracker.target = transform;
                    resourceNotification.gameObject.SetActive(true);
                    GetComponentInChildren<Renderer>().material.color = Color.green;
                    if (navObstacle)
                    {
                        navObstacle.enabled = false;
                    }
                    break;
                }
        }

        this.state = state;
    }

    public ItemData[] GetMissingIngredients()
    {
        List<ItemData> missingMaterials = new List<ItemData>();
        missingMaterials.AddRange(data.recipe);

        for (int i = 0; i < buildMaterials.Count; i++)
        {
            int foundIndex = missingMaterials.FindIndex(x => x == buildMaterials[i].data);
            if (foundIndex != -1)
            {
                missingMaterials.RemoveAt(foundIndex);
            }
        }

        return missingMaterials.ToArray();
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        if (state != State.Building)
        {
            yield return null;
            yield break;
        }

        yield return unit.navAgent.MoveToInteractableSeq(GetComponent<Interactable>());

        if (buildMaterials.Count < data.recipe.Length)
        {
            ItemData[] missingIngredients = GetMissingIngredients();
            for (int i = 0; i < missingIngredients.Length; i++)
            {
                Item foundItem = Item.FindItem(missingIngredients[i]);
                if (foundItem != null)
                {
                    Storable storable = foundItem.GetComponent<Storable>();
                    yield return storable.PickUpInteraction(unit);
                    yield return unit.navAgent.MoveToInteractableSeq(GetComponent<Interactable>());
                    if (state != State.Normal)
                    {
                        unit.inventory.Withdraw(storable);
                        AddBuildMaterial(foundItem);
                    }

                    yield break;
                }
            }
        }
        else
        {
            CompleteBuilding();
        }
        yield return new WaitForSeconds(2f);

        // Keep building the other buildings nearby
        Collider[] nearby = Physics.OverlapSphere(unit.transform.position, 10f);
        if (nearby != null)
        {
            for (int i = 0; i < nearby.Length; i++)
            {
                Building nearbyBuilding = nearby[i].GetComponent<Building>();
                if (nearbyBuilding && nearbyBuilding.state == State.Building)
                {
                    yield return nearbyBuilding.DoInteraction(unit);
                }
            }
        }
        yield break;
    }

    public void AddBuildMaterial(Item item)
    {
        if (item != null)
        {
            item.GetComponent<Storable>().state = Storable.State.InUse;
            buildMaterials.Add(item);
            item.gameObject.SetActive(false);
        }

        float yScale = (float)buildMaterials.Count / (data.recipe.Length + 1f);
        Tweener buildTweener = transform.DOScaleY(yScale, 1f).SetEase(Ease.InOutElastic);
        resourceNotification.Set(GetMissingIngredients());

        if (buildMaterials.Count == data.recipe.Length)
        {
            state = State.Normal;
            buildTweener.onComplete += () =>
            {
                CompleteBuilding();
            };
        }
    }

    public void CompleteBuilding()
    {
        SetState(State.Normal);
        transform.DOScale(1f, 1f).SetEase(Ease.InOutElastic);
        UI.ShowNotification(GetComponent<Interactable>(), $"{data.name} Complete!");
        resourceNotification.gameObject.SetActive(false);
    }

    public bool IsEnabled()
    {
        return state == State.Building;
    }

    public string GetName()
    {
        return "Build";
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>("Icons/Placeholders/hammer");
    }
}
