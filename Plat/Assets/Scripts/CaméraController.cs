using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaméraController : MonoBehaviour
{

    public float camSpeed;
    public float border;

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < border || Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * Time.deltaTime * camSpeed;
        if(Input.mousePosition.x > Screen.width-border || Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * camSpeed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -18, 18), transform.position.y,transform.position.z);
       
    }
}
