using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class HierarchyUtils
    {
        public static List<T> GetComponentInDirectChildren<T>(Transform parent)
            where T : Component
        {
            List<T> Tlist = new List<T>();
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).gameObject.activeInHierarchy) 
                Tlist.Add(parent.GetChild(i).GetComponent<T>());
            }
            return Tlist;
        }
    }
}

