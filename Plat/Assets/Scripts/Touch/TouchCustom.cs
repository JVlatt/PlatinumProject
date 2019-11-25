using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchCustom 
{


    public abstract void Update(Touch touch,Touch nextTouch = new Touch());
    public virtual void End(){ }

    public abstract TYPE GetToucheType();


    public enum TYPE{
        SAMPLE,
        SLIDE,
        PINCH,
    }
}
