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
                m_animator.SetBool("isHealing", true);
                transform.forward = transform.position - m_peonToHeal.transform.position;
                _ACTIVITY = ACTIVITY.HEAL;
                _particle.Play();
                _particle.transform.position = value.transform.position;
            }
            else {
                transform.forward = Vector3.forward ;
                if (!_isFixing)
                    m_animator.SetBool("isHealing", false);
                _particle.Stop();
                _particle.transform.position = transform.position;
            }
        }
    }
    private float _timer;
    private Infirmary m_infirmary;
    private Infirmary _infirmary
    {
        get { return m_infirmary; }
        set
        {
            m_infirmary = value;
        }
    }
    private ParticleSystem _particle;

    public override void SpecialStart()
    {
        base.SpecialStart();
        _particle = GetComponentInChildren<ParticleSystem>();
        _particle.Stop();
    }

    public override void SpecialUpdate()
    {
        if (m_infirmary != null && _peonToHeal == null && _ACTIVITY != ACTIVITY.WAIT && _isFixing != true)
            _ACTIVITY = ACTIVITY.WAIT;
        else if (m_infirmary == null && _ACTIVITY != ACTIVITY.NONE && _isFixing !=true)
            _ACTIVITY = ACTIVITY.NONE;
        if (_peonToHeal && _infirmary && !_canMove && !_isFixing)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _peonToHeal.TreatPeon();
                _peonToHeal = _infirmary.GetPeonToHeal();
                _timer = cdHeal;
            }

        }
        else if (!_peonToHeal && _infirmary && !_canMove && !_isFixing)
        {
            _peonToHeal = _infirmary.GetPeonToHeal();
        }
    }



    public void Setup(Infirmary Infirmary,Peon peon) 
    {
        _peonToHeal = peon;
        _infirmary = Infirmary;
        _timer = cdHeal;
    }
        
    public bool RemoveOrUpdateHealer(Peon peon,Peon lowerPeon)
    {
        if(peon._ID == this._ID) //supprime le Healer si c'est le Peon a supprimer
        {
            _infirmary.AddPeonToHeal(_peonToHeal);
            _peonToHeal = null;
            _timer = cdHeal;
            _infirmary = null;
            return true;
        }
        else
        {
            if(_peonToHeal &&  peon._ID == _peonToHeal._ID) //met a jours les Healer si ils etaient en train de soigner le Peon a supprimer
            {
                _peonToHeal = lowerPeon;
                _timer = cdHeal;
                _infirmary.RemovePeonToHeal(lowerPeon);
            }
            return false;
        }
    }

    public override bool CanFix(Carriage carriage)
    {
        if (_peonToHeal && carriage == _infirmary)
            return false;
        else
            return base.CanFix(carriage);
    }

    public override void MovePeon(Carriage carriage)
    {
        base.MovePeon(carriage);
        _timer = cdHeal;
    }

}
