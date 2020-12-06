using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{
    public enum State
    {
        Normal = 0,
        Building = 1,
    }

    new public string name;
    public State state;
    public NavMeshObstacle navObstacle;

    public void SetState(State state)
    {
        switch (state)
        {
            case State.Normal:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                navObstacle.enabled = true;
                break;
            case State.Building:
                GetComponentInChildren<Renderer>().material.color = Color.green;
                navObstacle.enabled = false;
                break;
        }
    }
}
