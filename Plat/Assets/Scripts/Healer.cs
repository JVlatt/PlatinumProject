using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Peon  //MADE BY CEDRIC
{  
    [SerializeField]
    private float cdHeal;

    private Peon _peonToHeal;
    private float _timer;
    private Infirmary _Infirmary;


    public override void SpecialUpdate()
    {
        if (_peonToHeal && _Infirmary)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _peonToHeal.TreatPeon();
                _peonToHeal = _Infirmary.GetPeonToHeal();
                _timer = cdHeal;
            }

        }
        else if (!_peonToHeal && _Infirmary)
        {
            _peonToHeal = _Infirmary.GetPeonToHeal();
        }
    }



    public void Setup(Infirmary Infirmary,Peon peon) 
    {
        _Infirmary = Infirmary;
        _peonToHeal = peon;
        _timer = cdHeal;
    }
        
    public bool RemoveOrUpdateHealer(Peon peon,Peon lowerPeon)
    {
        if(peon._ID == this._ID) //supprime le Healer si c'est le Peon a supprimer
        {
            _Infirmary.AddPeonToHeal(_peonToHeal);
            _peonToHeal = null;
            _timer = cdHeal;
            _Infirmary = null;
            return true;
        }
        else
        {
            if(peon._ID == _peonToHeal._ID) //met a jours les Healer si ils etaient en train de soigner le Peon a supprimer
            {
                _peonToHeal = lowerPeon;
                _timer = cdHeal;
                _Infirmary.RemovePeonToHeal(lowerPeon);
            }
            return false;
        }
    }


}
