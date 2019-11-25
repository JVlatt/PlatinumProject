using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchTouch : TouchCustom
{
    private bool _isActive = false;
    private int _ID1;
    private int _ID2;
    private bool fingerUp;

    public PinchTouch(int ID1, int ID2)
    {
        _isActive = !PinchPara.haveActivePinch;
        PinchPara.haveActivePinch = true;
        _ID1 = ID1;
        _ID2 = ID2;
    }

    public override void End()
    {
        if (_isActive)
            PinchPara.haveActivePinch = false;
        fingerUp = true;
    }

    public override TYPE GetToucheType()
    {
        if (fingerUp)
            return TYPE.SAMPLE;
        else
            return TYPE.PINCH;
    }

    public override void Update(Touch touch, Touch otherTouch = new Touch())
    {
        if (PinchPara.haveActivePinch && !_isActive || fingerUp) return;
        if (!PinchPara.haveActivePinch)
        {
            _isActive = true;
            PinchPara.haveActivePinch = true;
        }


        float angle = Vector2.Angle(touch.deltaPosition, otherTouch.position-touch.position);
        if (0<angle&& angle < 30)
            TouchController.pinchDelegate(-touch.deltaPosition.magnitude*PinchPara._pinchSpeed);
        else if (angle > 150)
            TouchController.pinchDelegate(touch.deltaPosition.magnitude* PinchPara._pinchSpeed);
    }

    public int GetOtherID(int id)
    {
        if (id == _ID1)
        {
            return TouchController.GetTouchIndex(_ID2);
        }
        else
            return TouchController.GetTouchIndex(_ID1); ;
    }

    private TouchController.PinchPara PinchPara
    {
        get { return TouchController.pinchPara; }
    }

}
