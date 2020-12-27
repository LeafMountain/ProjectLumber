using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectTracker : MonoBehaviour
{
    public Transform target;
    public Vector3 worldOffset;
    public Vector2 offest;

    void LateUpdate()
    {
        if (target)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position + worldOffset) + (Vector3)offest;
        }
    }
}
