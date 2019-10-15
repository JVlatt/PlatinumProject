using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class Infirmary : Carriage  //MADE BY CEDRIC
{
    private List<Healer> healers = new List<Healer>();
    private List<Peon> PeonsToHeal = new List<Peon>();


    public override void AddPeonToSpecialCarriage(Peon peon)
    {
        if (peon._HEALTHSTATE == Peon.HEALTHSTATE.HURT)
            AddPeonToHeal(peon);
        if(peon._type == Peon.TYPE.HEALER)
        {
            Healer newHealer = peon.GetComponent<Healer>();
            newHealer.Setup(this, PeonsToHeal[0]);
            PeonsToHeal.RemoveAt(0);
            healers.Add(newHealer);
        }
    }

    public override void RemovePeon(Peon peon)
    {
        base.RemovePeon(peon);
 
        if (peon._HEALTHSTATE == Peon.HEALTHSTATE.HURT)
            PeonsToHeal.Remove(peon);

        //supprime le Healer si c'est le Peon a supprimer et met a jours les Healer si ils etaient en train de soigner le Peon a supprimer
        Healer ToRemove=null;
        foreach (Healer item in healers)
        {
            if (item.RemoveOrUpdateHealer(peon, PeonsToHeal[0]))
            {
                ToRemove = item;
            }
        }
        healers.Remove(ToRemove);
    }


    public void AddPeonToHeal(Peon peon)
    {
        int i = 0;
        while (peon.PVLost()<PeonsToHeal[i].PVLost())
        {
            i++;
        }
        PeonsToHeal.Insert(i, peon);
    }

    public void RemovePeonToHeal(Peon peon) { PeonsToHeal.Remove(peon); }

    public Peon GetPeonToHeal()
    {
        Peon toHeal = PeonsToHeal[0];
        PeonsToHeal.RemoveAt(0);
        return toHeal;
        
    }
}
