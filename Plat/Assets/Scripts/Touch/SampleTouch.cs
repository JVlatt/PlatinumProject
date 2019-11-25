using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTouch : TouchCustom
{

    private Collider _touchedCollider;
    private float _distanceMax;
    private Vector2 _currentDistance;

    public SampleTouch(Collider touchedCollider,float distanceMax)
    {
        _touchedCollider = touchedCollider;
        _distanceMax = distanceMax;
    }

    public override void End()
    {
        if (_touchedCollider != null && _currentDistance.magnitude < _distanceMax)
            _touchedCollider.SendMessage("Touch");
    }

    public override TYPE GetToucheType()
    {
        return TYPE.SAMPLE;
    }

    public override void Update(Touch touch, Touch nextTouch = new Touch())
    {
        _currentDistance += touch.deltaPosition;
        if (touch.phase == TouchPhase.Moved && _currentDistance.magnitude>_distanceMax && (TouchController.Instance.mask & 1<<1)!=0)
            TouchController.Instance.ConvertSampleToSlide(this,touch); //convert to slide
    }
}
