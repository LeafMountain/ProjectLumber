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

    // Prefabs
    public GameObject hoverMarker;
    public GenericUIElement hoverTooltip;
    public GameObject selectionMarker;

    [Header("Read only")]
    public Interactable hoverInteractable = null;
    public GameObject hoverUI;
    public RaycastHit mouseHit;

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

        Collider collider = interactable.GetComponent<Collider>();
        Instance.selectionMarker.transform.localScale = collider.bounds.size * 2f;
        //Instance.selectionMarker.transform.localPosition = collider.bounds.min + new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);
        Instance.selectionMarker.transform.SetParent(interactable.transform);
        Instance.selectionMarker.transform.localPosition = Vector3.zero;
        Instance.selectionMarker.SetActive(true);
    }

    public static void RemoveInteractable(Interactable interactable)
    {
        Instance.interactables.Remove(interactable);
        interactable.OnDeselected();
        OnUpdated?.Invoke();

        Instance.selectionMarker.SetActive(false);
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
        if (overUI == false)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
            {
                Interactable newHoverInteractable = mouseHit.transform?.GetComponent<Interactable>();
                if (hoverInteractable != null && hoverInteractable != mouseHit.transform)
                {
                    hoverInteractable.OnUnhover();
                    hoverMarker.transform.SetParent(null);
                    hoverMarker.SetActive(false);
                }
                hoverInteractable = newHoverInteractable;
                if (hoverInteractable != null)
                {
                    hoverInteractable.OnHover();
                    hoverMarker.transform.localScale = mouseHit.collider.bounds.size * 2f;
                    hoverMarker.transform.localPosition = mouseHit.collider.bounds.min + new Vector3(mouseHit.collider.bounds.extents.x, 0, mouseHit.collider.bounds.extents.z);
                    hoverMarker.transform.SetParent(hoverInteractable.transform);
                    hoverMarker.SetActive(true);
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
                            unit.navAgent.SetDestination(mouseHit.point);
                            unit.currentInteraction = null;
                        }
                    }
                }
            }
        }

        // Get hover over UI
        if(overUI)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, result);
            if(result.Count > 0)
            {
                hoverUI = result[0].gameObject;
                Debug.Log(hoverUI);
            }
        }

        // Show hover tooltip
        if (overUI && hoverUI)
        {
            IUIHoverInfo hoverInfo = hoverUI.GetComponent<IUIHoverInfo>();
            if (hoverInfo != null)
            {
                hoverTooltip.text.SetText(hoverInfo.GetTooltip());
                hoverTooltip.transform.position = hoverUI.transform.position + new Vector3(0f, -40f);
                hoverTooltip.gameObject.SetActive(true);
            }
        }
        else if (hoverInteractable)
        {
            IUIHoverInfo hoverInfo = hoverInteractable.GetComponent<IUIHoverInfo>();
            if (hoverInfo != null)
            {
                hoverTooltip.text.SetText(hoverInfo.GetTooltip());
                //hoverTooltip.transform.position = Input.mousePosition + new Vector3(40f, 20f);    // Mouse position
                hoverTooltip.transform.position = Camera.main.WorldToScreenPoint(mouseHit.collider.bounds.max);
                hoverTooltip.gameObject.SetActive(true);
            }
        }
        else
        {
            hoverTooltip.gameObject.SetActive(false);
        }
    }

    public static void SendInteraction(IInteractable interaction)
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
