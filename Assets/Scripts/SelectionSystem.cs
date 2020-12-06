using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance;
    public Interactable selected;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(this);
            Debug.Log("Selection system already exists");
        }
        Instance = this;
    }
}
