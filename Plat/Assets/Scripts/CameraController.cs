﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float camSpeed;
    [SerializeField]
    private float mouseBorder;
    [SerializeField]
    private float borderLeft;
    [SerializeField]
    private float borderRight;
    [SerializeField]
    private Zoom zoom;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < mouseBorder || Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * Time.deltaTime * camSpeed;
        if(Input.mousePosition.x > Screen.width-mouseBorder || Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * camSpeed;
        

        if (Input.mouseScrollDelta.y > 0)
            transform.position -= Vector3.back * Time.deltaTime * zoom.speed;
        if (Input.mouseScrollDelta.y < 0)
            transform.position -= Vector3.forward * Time.deltaTime * zoom.speed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, borderLeft, borderRight), transform.position.y, Mathf.Clamp(transform.position.z,zoom.min,zoom.max));

    }

    [System.Serializable]
    private struct Zoom
    {
        public float min;
        public float max;
        public float speed;
    }
}
