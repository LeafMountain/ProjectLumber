using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Building : MonoBehaviour, IUnitInteractable
{
    public enum State
    {
        Normal = 0,
        Building = 1,
    }

    new public string name;
    public State state;
    public NavMeshObstacle navObstacle;
    public Item[] recipe;

    public List<Item> buildMaterials = new List<Item>();

    public void SetState(State state)
    {
        switch (state)
        {
            case State.Normal:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                navObstacle.enabled = true;
                break;
            case State.Building:
                GetComponentInChildren<Renderer>().material.color = Color.green;
                navObstacle.enabled = false;
                break;
        }

        this.state = state;
    }

    public IEnumerator AttackInteraction(Unit unit)
    {
        yield return unit.navAgent.MoveToPosition(transform.position);
        unit.animator.SetTrigger("attackMelee");
        yield return new WaitForSeconds(.5f);
        GetComponent<Health>().Damage(-1, unit.interactable);
        yield return new WaitForSeconds(.2f);
    }

    public IEnumerator BuildInteraction(Unit unit)
    {
        if(state != State.Building)
        {
            yield return null;
            yield break;
        }

        if(buildMaterials.Count < recipe.Length)
        {
            for (int i = 0; i < recipe.Length; i++)
            {
                if(buildMaterials.IndexOf(recipe[i]) != -1)
                {
                    continue;
                }
                Item foundItem = Item.FindItem(recipe[i]);
                if(foundItem != null)
                {
                    Storable storable = foundItem.GetComponent<Storable>();
                    yield return storable.PickUpInteraction(unit);
                    yield return unit.navAgent.MoveToInteractable(GetComponent<Interactable>());
                    if(state != State.Normal)
                    {
                        unit.inventory.Withdraw(storable);
                        AddBuildMaterial(foundItem);
                    }

                    yield break;
                }
            }
        }
        yield return null;
        yield break;
    }

    public IEnumerator DoInteraction(Unit unit)
    {
        if(state == State.Building)
        {
            yield return BuildInteraction(unit);
        }
        else if(state == State.Normal)
        {
            yield return GetComponent<Health>().AttackInteraction(unit);
        }
    }

    public void AddBuildMaterial(Item item)
    {
        item.GetComponent<Storable>().state = Storable.State.InUse;
        buildMaterials.Add(item);
        item.gameObject.SetActive(false);

        float yScale = (float)buildMaterials.Count / (recipe.Length + 1f);
        transform.DOScaleY(yScale, 1f).SetEase(Ease.InOutElastic).onComplete += () =>
        {
            if (buildMaterials.Count == recipe.Length)
            {
                transform.DOScale(1f, 1f).SetEase(Ease.InOutElastic);
                // Building done!
                SetState(State.Normal);
                UINotification.ShowNotification(GetComponent<Interactable>(), $"{name} Complete!");
            }
        };
    }
}
