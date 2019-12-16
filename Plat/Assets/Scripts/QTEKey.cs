using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEKey : MonoBehaviour
{
    QTERepair _qte;
    [HideInInspector]
    public bool valid = true;
    public Vector3 startPosition;
    public Transform anchor;
    public float offset;

    private void Awake()
    {
        _qte = GetComponentInParent<QTERepair>();
        
        valid = false;
    }
    private void Start()
    {
        startPosition = transform.position;
    }
#if  UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void OnMouseOver()
    {
        if (!(Input.GetMouseButton(0) || Input.GetMouseButton(1))) return;
        if (!_qte.isActive || valid) return;
        

        Vector3 desiredPosition = Input.mousePosition;
        desiredPosition.z = -Camera.main.transform.position.z + transform.position.z;
        desiredPosition = Camera.main.ScreenToWorldPoint(desiredPosition);
        transform.position = desiredPosition;
    }

    private void OnMouseEnter()
    {
        UIManager.Instance.ChangeCursor("overlay");
    }

    private void OnMouseExit()
    {
        UIManager.Instance.ChangeCursor("default");
    }
#endif
#if UNITY_EDITOR || UNITY_ANDROID

    private void AddToDelegate()
    {
        TouchController.dragDelegate = Drag;
    }

    private void Drag(Vector3 position)
    {
        if (!_qte.isActive || valid) return;
        transform.position = position;
    }
#endif
    private void Update()
    {
        if (!valid)
        {
            float dist = Vector3.Distance(anchor.position, transform.position);
            if (dist <= 0.5f)
            {
                valid = true;
                SoundManager.Instance.Play("cling");
                transform.position = anchor.position;
                _qte.CheckEnd();
            }
        }
    }
    private void LateUpdate()
    {
        Vector3 desiredPosition = transform.position;
        desiredPosition.x =Mathf.Clamp(desiredPosition.x, _qte.topLeft.position.x, _qte.bottomRight.position.x);
        desiredPosition.y =Mathf.Clamp(desiredPosition.y, _qte.bottomRight.position.y, _qte.topLeft.position.y);
        transform.position = desiredPosition;
    }

    public void Reset()
    {
        transform.position = startPosition;
    }
}
