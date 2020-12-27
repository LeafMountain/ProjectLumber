using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionSystem : MonoBehaviour
{
    public delegate void InteractionEvent();
    public static InteractionEvent OnUpdated;

    public static InteractionSystem Instance;
    public List<Interactable> interactables = new List<Interactable>();
    public Interactable hoverInteractable = null;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            Debug.Log("Selection system already exists");
        }
        Instance = this;
    }

    public static void AddInteractable(Interactable interactable)
    {
        Instance.interactables.Add(interactable);
        interactable.OnSelected();
        OnUpdated?.Invoke();
    }

    public static void RemoveInteractable(Interactable interactable)
    {
        Instance.interactables.Remove(interactable);
        interactable.OnDeselected();
        OnUpdated?.Invoke();
    }

    public static void ClearSelection()
    {
        for (int i = Instance.interactables.Count - 1; i >= 0; i--)
        {
            RemoveInteractable(Instance.interactables[i]);
        }
        InteractionMenu.CloseMenu();
    }

    public void Update()
    {
        bool overUI = EventSystem.current.IsPointerOverGameObject();
        // Get Hover target
        RaycastHit hit = default;
        if (overUI == false)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Interactable newHoverInteractable = hit.transform?.GetComponent<Interactable>();
                if (hoverInteractable != null && hoverInteractable != hit.transform)
                {
                    hoverInteractable.OnUnhover();
                }
                hoverInteractable = newHoverInteractable;
                if (hoverInteractable != null)
                {
                    hoverInteractable.OnHover();
                }
            }
        }
        else
        {
            hoverInteractable = null;
        }

        if (hoverInteractable)
        {
            // Left mouse button
            if (Input.GetMouseButtonDown(0))
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    ClearSelection();
                }
                else if (interactables.Contains(hoverInteractable))
                {
                    RemoveInteractable(hoverInteractable);
                }
                AddInteractable(hoverInteractable);
            }
            // Right mouse button
            else if (Input.GetMouseButtonDown(1))
            {
                InteractionMenu.OpenMenu(hoverInteractable);
            }
        }
        else if (overUI == false)
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) == false)
            {
                ClearSelection();
            }

            // Move
            if (Input.GetMouseButtonDown(1))
            {
                if (interactables.Count > 0)
                {
                    for (int i = 0; i < interactables.Count; i++)
                    {
                        if (interactables[i].GetComponent<Unit>() is Unit unit)
                        {
                            unit.navAgent.SetDestination(hit.point);
                            unit.currentInteraction = null;
                        }
                    }
                }
            }
        }

    }

    public static void SendInteraction(IUnitInteractable interaction)
    {
        for (int i = 0; i < Instance.interactables.Count; i++)
        {
            if (Instance.interactables[i].GetComponent<IInteractionReceiver>() is IInteractionReceiver receiver)
            {
                receiver.ReceiveInteraction(interaction);
            }
        }
    }
}
