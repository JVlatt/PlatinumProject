using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infirmary : Carriage  //MADE BY CEDRIC
{
    private List<Healer> healers = new List<Healer>();
    private List<Peon> _peonsToHeal = new List<Peon>();


    public override void AddPeonToSpecialCarriage(Peon peon)
    {
        base.AddPeonToSpecialCarriage(peon);
        if (peon._HEALTHSTATE == Peon.HEALTHSTATE.HURT)
            AddPeonToHeal(peon);
        if(peon._type == Peon.TYPE.HEALER)
        {
            Healer newHealer = (Healer)peon;
            healers.Add(newHealer);
            newHealer.Setup(this, newHealer._isFixing?null:GetPeonToHeal());
        }
    }

    public override void RemovePeon(Peon peon)
    {
        base.RemovePeon(peon);
 
        if (peon._HEALTHSTATE == Peon.HEALTHSTATE.HURT)
            _peonsToHeal.Remove(peon);

        //supprime le Healer si c'est le Peon a supprimer et met a jours les Healer si ils etaient en train de soigner le Peon a supprimer
        Healer ToRemove=null;
        foreach (Healer item in healers)
        {
            if (item.RemoveOrUpdateHealer(peon, _peonsToHeal.Count!=0 ? _peonsToHeal[0] : null ))
            {
                ToRemove = item;
            }
        }
        healers.Remove(ToRemove);
    }


    public void AddPeonToHeal(Peon peon)
    {
        if (_peonsToHeal.Count == 0) { _peonsToHeal.Add(peon); return; }

        int i = 0;
        while (peon.HPLost()<_peonsToHeal[i].HPLost())
        {
            i++;
        }
        _peonsToHeal.Insert(i, peon);
    }

    public void RemovePeonToHeal(Peon peon) { _peonsToHeal.Remove(peon); }

    public Peon GetPeonToHeal()
    {
        if (_peonsToHeal.Count == 0) return null;

        Peon toHeal = _peonsToHeal[0];
        _peonsToHeal.RemoveAt(0);
        return toHeal;
        
    }
}
