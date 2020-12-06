using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    // Static
    public static List<UIElement> openElements = new List<UIElement>();
    public static UIElement TopElement => openElements.Count > 0 ? openElements[openElements.Count - 1] : null;
    
    public static void OpenElement(UIElement uIElement)
    {
        int existingIndex = openElements.IndexOf(uIElement);
        if(existingIndex != -1)
        {
            TopElement.topElement = false;
            openElements.RemoveAt(existingIndex);
            openElements.Add(uIElement);
            uIElement.topElement = true;
            return;
        }

        if(TopElement != null)
        {
            TopElement.topElement = false;
        }
        openElements.Add(uIElement);
        uIElement.topElement = true;
    }

    public static void CloseTopElement()
    {
        if(openElements.Count > 0)
        {
            TopElement.topElement = false;
            TopElement.Close();
            // openElements.RemoveAt(openElements.Count - 1);
        }
    }

    public static void CloseElement(UIElement uIElement)
    {
        int index = openElements.IndexOf(uIElement);
        if(index != -1)
        {
            openElements[index].topElement = false;
            openElements.RemoveAt(index);
        }
    }

    // Instance
    public bool topElement;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        OpenElement(this);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        CloseElement(this);
    }

    protected virtual void Update()
    {
        if(topElement && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
}
