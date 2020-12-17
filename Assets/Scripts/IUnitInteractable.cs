using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitInteractable
{
    public IEnumerator DoInteraction(Unit unit);
}
