using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton
    private static CameraController _instance = null;

    public static CameraController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private float camSpeed;
    [SerializeField]
    private float mouseBorder;
    [SerializeField]
    private MinMax zoom;
    [SerializeField]
    private MinMax y;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float travelSpeed;
    private MinMax borderLeft = new MinMax();
    private MinMax currentBorderLeft = new MinMax();
    private MinMax borderRight = new MinMax();
    private MinMax currentBorderRight = new MinMax();

    private Vector3 _target;
    private bool _isMoving = false;
    private Vector3 _startPosition;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            _instance = this;
    }

    private void Start()
    {
        _startPosition = transform.position;
        currentBorderLeft = borderLeft;
        currentBorderRight = borderRight;
    }

    public void MajCamera(List<Carriage> carriages)
    {
        if (carriages.Count == 1)
        {
            borderRight.min = carriages[0].transform.position.x;
            borderRight.max = carriages[0].transform.position.x;
            borderLeft.min = carriages[0].transform.position.x;
            borderLeft.max = carriages[0].transform.position.x;
        }
        else
        {
            borderRight.min = carriages[0].transform.position.x - (y.min - y.max);
            borderRight.max = carriages[0].transform.position.x;
            borderLeft.min = carriages[carriages.Count - 1].transform.position.x + (y.min - y.max);
            borderLeft.max = carriages[carriages.Count - 1].transform.position.x;
        }
    }

    void Update()
    {
        if(!_isMoving)
        {
            if(!PhaseManager.Instance.activePhase.freezeControl)
            {
                if (Input.mousePosition.x < mouseBorder || Input.GetKey(KeyCode.Q))
                    transform.position += Vector3.left * Time.deltaTime * camSpeed;
                if (Input.mousePosition.x > Screen.width - mouseBorder || Input.GetKey(KeyCode.D))
                    transform.position += Vector3.right * Time.deltaTime * camSpeed;


                if (Input.mouseScrollDelta.y > 0)
                    transform.position -= Vector3.back * Time.deltaTime * zoomSpeed;
                if (Input.mouseScrollDelta.y < 0)
                    transform.position -= Vector3.forward * Time.deltaTime * zoomSpeed;

                float lerpRatio = (zoom.min - transform.position.z) / (zoom.min - zoom.max);
                float left = Mathf.Lerp(currentBorderLeft.min, currentBorderLeft.max, lerpRatio);
                float right = Mathf.Lerp(currentBorderRight.min, currentBorderRight.max, lerpRatio);

                currentBorderLeft = MinMax.Lerp(currentBorderLeft, borderLeft, 0.1f);
                currentBorderRight = MinMax.Lerp(currentBorderRight, borderRight, 0.1f);

                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, left, right),
                    Mathf.Lerp(y.min, y.max, lerpRatio),
                    Mathf.Clamp(transform.position.z, zoom.min, zoom.max)
                    );
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * travelSpeed);
            if(Vector3.Distance(_target, transform.position) <= 0.5f)
            {
                PhaseManager.Instance.NextPhase();
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

    public void ResetCamera()
    {
        _target = _startPosition;
        _isMoving = true;
    }
    [System.Serializable]
    private struct MinMax
    {
        public float min;
        public float max;

        public static MinMax Lerp(MinMax a, MinMax b , float ratio)
        {
            a.min = Mathf.Lerp(a.min, b.min, ratio);
            a.max = Mathf.Lerp(a.max, b.min, ratio);
            return a;
        }
    }
}
