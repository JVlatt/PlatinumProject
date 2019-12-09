using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTouch : TouchCustom
{
    private Collider _collider;
    private bool _isActive;
    private bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            if (value)
            {
                _collider.SendMessage("AddToDelegate");
                DragPara.haveActiveDrag = false;
            }
        }
    }

    public override TYPE GetToucheType()
    {
        return TYPE.DRAG;
    }

    public DragTouch(Collider collider)
    {
        _collider = collider;
        IsActive = !DragPara.haveActiveDrag;
        DragPara.haveActiveDrag = true;
    }

    public override void End()
    {
        if (IsActive)
            DragPara.haveActiveDrag = false;
    }

    public override void Update(Touch touch, Touch nextTouch = default)
    {
        if (DragPara.haveActiveDrag && !IsActive) return;
        if (!DragPara.haveActiveDrag)
            IsActive = true;
        TouchController.dragDelegate(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,-Camera.main.transform.position.z)));
    }

    private TouchController.DragPara DragPara
    {
        get { return TouchController.dragPara; }
    }
}
