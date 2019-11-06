using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : MonoBehaviour
{

    Carriage _carriage;

    private void Start()
    {
        _carriage = GetComponentInParent<Carriage>();
    }


    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {

        }
    }
}
