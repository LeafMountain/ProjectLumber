using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public delegate void InteractableEvent(Interactable interactable);
    public static InteractableEvent OnSelection;
    public static InteractableEvent OnDeselection;

    public bool hover;
    public bool selected;

    public void OnSelected()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
        selected = true;
    }

    public void OnDeselected()
    {
        GetComponentInChildren<Renderer>().material.color = Color.white;
        selected = false;
    }

    public void OnHover()
    {
        hover = true;
    }

    public void OnUnhover()
    {
        hover = false;
    }

    public string GetName()
    {
        Unit unit = GetComponent<Unit>();
        if (unit)
        {
            return unit.name;
        }

        Building building = GetComponent<Building>();
        if (building)
        {
            return building.name;
        }

        Item item = GetComponent<Item>();
        if (item)
        {
            return item.data.name;
        }

        return name;
    }
}
