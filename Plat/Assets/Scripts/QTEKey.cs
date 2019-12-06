﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEKey : MonoBehaviour
{
    QTERepair _qte;
    [HideInInspector]
    public bool valid = false;
    [HideInInspector]
    public Vector3 startPosition;
    public Transform anchor;

    private void Awake()
    {
        _qte = GetComponentInParent<QTERepair>();
        startPosition = transform.position;
    }

    private void OnMouseOver()
    {
        if (!(Input.GetMouseButton(0) || Input.GetMouseButton(1))) return;
        if (!_qte.isActive || valid) return;
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(distance);
        transform.position = new Vector3(rayPoint.x, rayPoint.y, transform.position.z);
    }

    private void Update()
    {
        if(!valid)
        {
            float dist = Vector3.Distance(anchor.transform.position, transform.position);
            if(dist <= 0.5f)
            {
                valid = true;
                transform.position = anchor.position;
                _qte.CheckEnd();
            }
        }
    }
}
