using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Peon
{

    public float cdHeal;

    private Peon _peonToHeal;
    private float _timer;
    private Infirmary _Infirmary;

    public void Setup(Infirmary Infirmary,Peon peon) 
    {
        _Infirmary = Infirmary;
        _peonToHeal = peon;
    }
        
    public bool RemoveHealer(Peon peon)
    {
        if(peon._ID == this._ID)
        {
            _peonToHeal = null;
            _timer = cdHeal;
            _Infirmary = null;
            return true;
        }
        else
        {
            if(peon._ID == _peonToHeal._ID)
            {
                _peonToHeal = null;
                _timer = cdHeal;
            }
            return false;
        }
    }
    


}
