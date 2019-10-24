using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Peon  //MADE BY CEDRIC
{  
    [SerializeField]
    private float cdHeal;

    private Peon m_peonToHeal;
    public Peon _peonToHeal
    {
        get { return m_peonToHeal; }
        set 
        { 
            m_peonToHeal = value;
            if (value)
            {
                _animator.SetBool("Healing", true);
                _particle.Play();
                _particle.transform.position = value.transform.position;
            }
            else { 
                if(!_isFixing)
                    _animator.SetBool("Healing", false);
                _particle.Stop();
                _particle.transform.position = transform.position;
            }
        }
    }
    private float _timer;
    private Infirmary _Infirmary;
    private ParticleSystem _particle;

    public override void SpecialStart()
    {
        base.SpecialStart();
        _particle = GetComponentInChildren<ParticleSystem>();
        _particle.Stop();
    }

    public override void SpecialUpdate()
    {
        if (_peonToHeal && _Infirmary && !_canMove && !_isFixing)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _peonToHeal.TreatPeon();
                _peonToHeal = _Infirmary.GetPeonToHeal();
                _timer = cdHeal;
            }

        }
        else if (!_peonToHeal && _Infirmary && !_canMove && !_isFixing)
        {
            _peonToHeal = _Infirmary.GetPeonToHeal();
        }
    }



    public void Setup(Infirmary Infirmary,Peon peon) 
    {
        _peonToHeal = peon;
        _Infirmary = Infirmary;
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

    public override bool CanFix(Carriage carriage)
    {
        if (_peonToHeal && carriage == _Infirmary)
            return false;
        else
            return base.CanFix(carriage);
    }

}
