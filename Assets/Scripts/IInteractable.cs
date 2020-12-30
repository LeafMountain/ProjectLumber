using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    IEnumerator DoInteraction(Unit unit);
    bool IsEnabled();
    string GetName();
    Sprite GetIcon();
    //void OnHover();
    //void OnUnhover();
    //void OnSelected();
    //void OnDeselected();
}
