using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

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
    [SerializeField]
    private float travelSpeed;

    private Vector3 _target;
    private bool _isMoving = false;

    private void Awake()
    {
        GameManager.GetManager().cameraController = this;
    }

    void Update()
    {
        if(!_isMoving)
        {
            if (Input.mousePosition.x < mouseBorder || Input.GetKey(KeyCode.A))
                transform.position += Vector3.left * Time.deltaTime * camSpeed;
            if (Input.mousePosition.x > Screen.width - mouseBorder || Input.GetKey(KeyCode.D))
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
                Mathf.Lerp(y.min, y.max, lerpRatio),
                Mathf.Clamp(transform.position.z, zoom.min, zoom.max)
                );
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * travelSpeed);
            if(Vector3.Distance(_target, transform.position) <= 0.5f)
            {
                GameManager.GetManager().phaseManager.NextPhase();
                _isMoving = false;
            }
        }

    }

    public void MoveToCarriage(Carriage carriage)
    {
        Vector3 target = carriage.transform.position;
        target.z = zoom.max;
        _target = target;
        _isMoving = true;
    }

    [System.Serializable]
    private struct MinMax
    {
        public float min;
        public float max;
    }
}
