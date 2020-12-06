using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void InteractableEvent(Interactable interactable);
    public static InteractableEvent OnSelection;
    public static InteractableEvent OnDeselection;
    public static List<Interactable> currentSelection = new List<Interactable>();

    public static void AddSelectable(Interactable newSelection)
    {
        if(currentSelection.Contains(newSelection) == false)
        {
            currentSelection.Add(newSelection);
            newSelection.OnSelected();
            OnSelection?.Invoke(newSelection);
        }
    }

    public static void RemoveSelectable(Interactable interactable)
    {
        if(currentSelection.Remove(interactable))
        {
            interactable.OnDeselected();
            OnDeselection?.Invoke(interactable);
        }
    }

    public static void ClearSelection()
    {
        for (int i = 0; i < currentSelection.Count; i++)
        {
            RemoveSelectable(currentSelection[i]);
        }
    }

    public bool hover;
    public bool selected;

    public void OnMouseDown()
    {
        AddSelectable(this);
    }

    private void OnMouseEnter()
    {
        OnHover();
    }

    private void OnMouseExit()
    {
        OnUnhover();
    }

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

    private void Update()
    {
        if(selected)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform != transform)
                {
                    if(Input.GetKey(KeyCode.LeftShift) == false)
                    {
                        RemoveSelectable(this);
                    }
                }
            }

            if(Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

                foreach (var interactable in GetComponents<IRightClickInteraction>())
                {
                    interactable.OnRightClick(hit);
                }
            }
        }
    }

    public string GetName()
    {
        Unit unit = GetComponent<Unit>();
        if(unit)
        {
            return unit.name;
        }
        return null;
    }
}
