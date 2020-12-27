using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float scrollSpeed = 500f;
    public Camera camera;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime);

        if (BuildMenu.building == false)
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            scrollDelta *= scrollSpeed;
            scrollDelta *= Time.deltaTime;
            camera.transform.position += camera.transform.forward.normalized * -scrollDelta;
        }
    }
}
