using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float camSpeed;
    [SerializeField]
    private float mouseBorder;
    [SerializeField]
    private MinMax zoom;
    [SerializeField]
    private MinMax borderLeft;
    [SerializeField]
    private MinMax borderRight;
    [SerializeField]
    private MinMax y;
    [SerializeField]
    private float zoomSpeed;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < mouseBorder || Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * Time.deltaTime * camSpeed;
        if(Input.mousePosition.x > Screen.width-mouseBorder || Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * Time.deltaTime * camSpeed;
        

        if (Input.mouseScrollDelta.y > 0)
            transform.position -= Vector3.back * Time.deltaTime * zoomSpeed;
        if (Input.mouseScrollDelta.y < 0)
            transform.position -= Vector3.forward * Time.deltaTime * zoomSpeed;

        float lerpRatio = (zoom.min - transform.position.z) / (zoom.min - zoom.max);
        float left = Mathf.Lerp(borderLeft.min, borderLeft.max, lerpRatio);
        float right = Mathf.Lerp(borderRight.min, borderRight.max, lerpRatio);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, left, right), 
            Mathf.Lerp(y.min,y.max,lerpRatio), 
            Mathf.Clamp(transform.position.z,zoom.min,zoom.max)
            );

    }

    [System.Serializable]
    private struct MinMax
    {
        public float min;
        public float max;
    }
}
