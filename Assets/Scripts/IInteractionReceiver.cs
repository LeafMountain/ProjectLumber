using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionReceiver
{
    void ReceiveInteraction(IUnitInteractable interaction);
}
