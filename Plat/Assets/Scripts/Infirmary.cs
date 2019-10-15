using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class Infirmary : Carriage
{
    private Peon _lowerPeon;
    private List<Healer> healers = new List<Healer>();

    // Update is called once per frame
    void Update()
    {/*
        if(healers != null && _peons.Count>1)
        {
            if (peonToHeal != null)
            {
                timer -= Time.deltaTime;
                if (timer == 0)
                    ;//HealPeon
            }
            else
            {
                /*  NEED PEON PV
                Peon peonLow;
                foreach (var item in _peons)
                {
                    if (peonLow.hp > item.hp)
                        peonLow = item;
                }
                peonToHeal = peonLow;
                
            }

        }*/
    }

    public override void AddPeonToSpecialCarriage(Peon peon)
    {
        if (_lowerPeon._PV > peon._PV)
            _lowerPeon = peon;
        if(peon._type == Peon.TYPE.HEALER)
        {
            Healer newHealer = peon.GetComponent<Healer>();
            newHealer.Setup(this, _lowerPeon);
            healers.Add(newHealer);
        }
    }

    public override void RemovePeon(Peon peon)
    {
        base.RemovePeon(peon);
        Healer ToRemove=null;
        foreach (Healer item in healers)
        {
            if (item.RemoveHealer(peon))
                ToRemove = item;
        }
        healers.Remove(ToRemove);

        if(_lowerPeon._ID == peon._ID)
        {
            _lowerPeon = null;
            foreach (Peon item in _peons)
            {
                if (_lowerPeon == null)
                    _lowerPeon = item;
                else if(_lowerPeon._PV>item._PV)
                    _lowerPeon = item;
            }
        }


    }
}
