using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISelectable : MonoBehaviour
{
    public TMP_Text nameText;

    private void Awake()
    {
        Interactable.OnSelection += OnSelection;
        Interactable.OnDeselection += OnDeselection;
        gameObject.SetActive(false);
    }

    private void OnSelection(Interactable interactable)
    {
        nameText.SetText("Name: " + interactable.GetName());
        gameObject.SetActive(true);
    }

    private void OnDeselection(Interactable interactable)
    {
        if(Interactable.currentSelection.Count == 0)
        {
            gameObject.SetActive(false);
            nameText.SetText(string.Empty);
        }
    }
}
