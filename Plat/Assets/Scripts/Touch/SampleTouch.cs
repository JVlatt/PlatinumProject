using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTouch : TouchCustom
{

    private Collider _touchedCollider;
    private float _distanceMax;
    private Vector2 _currentDistance;
    private Vector3 position;

    public SampleTouch(Collider touchedCollider,float distanceMax)
    {
        _touchedCollider = touchedCollider;
        _distanceMax = distanceMax;
    }

    public override void End()
    {
        if (_touchedCollider != null && _currentDistance.magnitude < _distanceMax)
        {
            _touchedCollider.SendMessage("Touch");
            FXManager.CallDelegate("Touch", Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, -Camera.main.transform.position.z)));
        }
    }

    public override TYPE GetToucheType()
    {
        return TYPE.SAMPLE;
    }

    public override void Update(Touch touch, Touch nextTouch = new Touch())
    {
        position = touch.position;
        _currentDistance += touch.deltaPosition;
        if (touch.phase == TouchPhase.Moved && _currentDistance.magnitude>_distanceMax && (TouchController.Instance.mask & 1<<1)!=0)
            TouchController.Instance.ConvertSample(this,touch,_touchedCollider); //convert to slide or drag
    }
}
