using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTouch : TouchCustom
{
    private Vector2 _startPositon;
    private Vector2 _lastPositon;
    private bool _isActive = false;

    public SlideTouch(Vector2 startPositon)
    {
        _startPositon = startPositon;
        _isActive = !SlidePara.haveActiveSlide;
        SlidePara.haveActiveSlide = true;
    }

    public override void End()
    {
        if (_isActive)
            SlidePara.haveActiveSlide = false;
        if (!SlidePara._slideContinu && TouchController.slideDelegate != null)
            TouchController.slideDelegate((_lastPositon - _startPositon)*SlidePara._slideSpeed);
    }

    public override TYPE GetToucheType()
    {
        return TYPE.SLIDE;
    }

    public override void Update(Touch touch, Touch nextTouch = new Touch())
    {
        if (SlidePara.haveActiveSlide && !_isActive) return;
        _lastPositon = touch.position;
        if (!SlidePara._slideContinu) return;
        if (!SlidePara.haveActiveSlide)
        {
            _isActive = true;
            SlidePara.haveActiveSlide = true;
        }

        Vector2 dif;
        if (SlidePara._useDeltaSlidePos)
            dif = touch.deltaPosition;
        else
            dif= (touch.position - _startPositon)* touch.deltaTime;

        TouchController.slideDelegate(dif*SlidePara._slideSpeed);
    }

    private TouchController.SlidePara SlidePara
    {
        get { return TouchController.slidePara; }
    }
}
