using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISelectable : MonoBehaviour
{
    public TMP_Text[] nameText;

    private void Awake()
    {
        InteractionSystem.OnUpdated += OnUpdated;
        gameObject.SetActive(false);
    }

    private void OnUpdated()
    {
        bool interactablesExists = InteractionSystem.Instance.interactables.Count > 0;
        gameObject.SetActive(interactablesExists);
        if (interactablesExists)
        {
            for (int i = 0; i < nameText.Length; i++)
            {
                nameText[i].gameObject.SetActive(i < InteractionSystem.Instance.interactables.Count);
                if (i < InteractionSystem.Instance.interactables.Count)
                {
                    nameText[i].SetText(InteractionSystem.Instance.interactables[i].GetName());
                }
            }
        }
    }
}
