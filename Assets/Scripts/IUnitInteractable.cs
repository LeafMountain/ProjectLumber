using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitInteractable
{
    IEnumerator DoInteraction(Unit unit);
    bool IsEnabled();
    string GetName();
    Sprite GetIcon();
}
