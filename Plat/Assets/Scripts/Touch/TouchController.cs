using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    #region Singleton
    private static TouchController _instance = null;
    public static TouchController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        if ((mask & 1 << 1) != 0)
            slidePara = new SlidePara(_slideContinu, _useDeltaSlidePos, _slideSpeed);
        if ((mask & 1 << 2) != 0)
            pinchPara = new PinchPara(_pinchSpeed);
        if ((mask & 1 << 3) != 0)
            dragPara = new DragPara();

    }
    #endregion

    [Header("Parametre")]

    [SerializeField]
    private float touchTimeBuffer;
    private float touchTimer;

    [MaskAtt()]
    [SerializeField]
    private int _mask;
    public int mask
    {
        get { return _mask; }
    }

    #region Touche
    [MaskHideAtt("_mask", 0, "", false, true)]
    [SerializeField]
    private float _maxDistanceSampleTouch;
    #endregion

    #region Slide
    [MaskHideAtt("_mask", 1, "", false, true)]
    [SerializeField]
    private bool _slideContinu;


    [MaskHideAtt("_mask", 1, "_slideContinu", false, false)]
    [SerializeField]
    private bool _useDeltaSlidePos;

    [MaskHideAtt("_mask", 1, "", false, false)]
    [SerializeField]
    private float _slideSpeed;

    public class SlidePara
    {
        public bool _slideContinu { get; private set; }
        public bool _useDeltaSlidePos { get; private set; }
        public float _slideSpeed { get; private set; }
        public bool haveActiveSlide;

        public SlidePara(bool slideContinu, bool useDeltaSlidePos, float slideSpeed)
        {
            _slideContinu = slideContinu;
            _useDeltaSlidePos = useDeltaSlidePos;
            _slideSpeed = slideSpeed;
        }
    }
    public static SlidePara slidePara { get; private set; }
    #endregion

    #region Pinch

    [MaskHideAtt("_mask", 2, "", false, true)]
    [SerializeField]
    private float _pinchSpeed;

    public class PinchPara
    {
        public float _pinchSpeed { get; private set; }
        public bool haveActivePinch;

        public PinchPara(float pinchSpeed)
        {
            _pinchSpeed = pinchSpeed;
        }
    }

    public static PinchPara pinchPara;
    #endregion

    #region Drag

    [MaskHideAtt("_mask", 3, "", false, true)]
    [SerializeField]
    private string _dragTag;

    public class DragPara
    {
        public bool haveActiveDrag;
    }
    public static DragPara dragPara;
    #endregion

    private void OnValidate()
    {
        if ((mask & 1 << 1) != 0)
            slidePara = new SlidePara(_slideContinu, _useDeltaSlidePos, _slideSpeed);
        if ((mask & 1 << 2) != 0)
            pinchPara = new PinchPara(_pinchSpeed);
        if ((mask & 1 << 3) != 0)
            dragPara = new DragPara();
    }

    private List<TouchCustom> touches = new List<TouchCustom>();
    private List<TouchCustom> touchesToRemove = new List<TouchCustom>();
    private List<Touch> touchToCreate = new List<Touch>();


    void Update()
    {
        foreach (var item in touchesToRemove)
        {
            touches.Remove(item);
        }
        touchesToRemove.Clear();
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            TouchCustom touchCustom = null;
            if (i < touches.Count)
                touchCustom = touches[i];
            if (touch.phase == TouchPhase.Began)
            {
                touchToCreate.Add(touch);
                if (touch.fingerId < touches.Count)
                    touches.Insert(touch.fingerId, null);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touchCustom == null)
                    touchToCreate.Remove(touchToCreate.Find(x => x.fingerId == touch.fingerId));
                else
                {
                    touchCustom.End();
                    touchesToRemove.Add(touchCustom);
                }
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && touchCustom != null)
            {
                if (touchCustom.GetToucheType() == TouchCustom.TYPE.PINCH)
                {
                    touchCustom.Update(touch, Input.GetTouch((touchCustom as PinchTouch).GetOtherID(i)));
                }
                else
                    touchCustom.Update(touch);
            }else if(touch.phase == TouchPhase.Canceled)
            {

            }
        }


        CreateToucheUpdate();
        //Debug.Log("touch : " + Input.touchCount + " touches : " + touches.Count);
    }

    private void CreateToucheUpdate()
    {
        if (touchTimer > 0) { touchTimer -= Time.deltaTime; return; }
        touchTimer = touchTimeBuffer;
        if (touchToCreate.Count == 0) return;
        if (touchToCreate.Count >= 2 && (mask & 1 << 2) != 0)
        {
            AddTouchToList(touchToCreate[0], touchToCreate[1].fingerId);
            touchToCreate.RemoveRange(0, 2);
        }
        foreach (Touch item in touchToCreate)
        {
            AddTouchToList(item);
        }
        touchToCreate.Clear();
    }

    private void AddTouchToList(Touch touch, int otherTouchID = -1)
    {
        TouchCustom touchCustom;
        touchCustom = ChosenTouch(touch, otherTouchID);

        int touchID = touch.fingerId;
        if (touchID < touches.Count)
        {
            touches.RemoveAt(touchID);
            touches.Insert(touchID, touchCustom);
        }
        else
        {
            touches.Add(touchCustom);
        }

        if (otherTouchID != -1)
        {
            if (otherTouchID < touches.Count)
            {
                touches.RemoveAt(otherTouchID);
                touches.Insert(otherTouchID, touchCustom);
            }
            else
            {
                touches.Add(touchCustom);
            }
        }
    }

    private TouchCustom ChosenTouch(Touch touch, int otherTouchID = -1)
    {
        TouchCustom touchCustom = null;
        if (otherTouchID != -1)
        {
            touchCustom = new PinchTouch(touch.fingerId, otherTouchID);
        }
        else {
            if ((_mask & 1 << 0) != 0)
            {
                Collider collider = GetCollider(touch);
                if ((_mask & 1 << 2) != 0 && collider != null && collider.tag == _dragTag)
                    touchCustom = new DragTouch(collider);
                else
                    touchCustom = new SampleTouch(collider, _maxDistanceSampleTouch);
            }
            else if ((_mask & 1 << 1) != 0)
                touchCustom = new SlideTouch(touch.position);
        }
        return touchCustom;
    }

    private Collider GetCollider(Touch touch)
    {
        Vector3 touchPosFar = new Vector3(touch.position.x, touch.position.y, Camera.main.farClipPlane);
        Vector3 touchPosNear = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);

        Vector3 touchPosF = Camera.main.ScreenToWorldPoint(touchPosFar);
        Vector3 touchPosN = Camera.main.ScreenToWorldPoint(touchPosNear);

        RaycastHit hit;

        if (Physics.Raycast(touchPosN, touchPosF - touchPosN, out hit))
            return hit.collider;
        else
            return null;
    }

    public void ConvertSample(SampleTouch sample, Touch touch,Collider collider)
    {
        int index = touches.IndexOf(sample);
        touches.Remove(sample);
        if (collider != null && collider.tag == _dragTag)
            touches.Insert(index, new DragTouch(collider));
        else
            touches.Insert(index, new SlideTouch(touch.position));
    }

    public static int GetTouchIndex(int ID)
    {
        if (ID < Input.touchCount && Input.GetTouch(ID).fingerId == ID) return ID;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == ID)
                return i;
        }

        return -1;
    }



    public delegate void SlideDelegate(Vector2 distance);

    public static SlideDelegate slideDelegate;

    public delegate void PinchDelegate(float value);

    public static PinchDelegate pinchDelegate;

    public delegate void DragDelegate(Vector3 position);

    public static DragDelegate dragDelegate;
}
