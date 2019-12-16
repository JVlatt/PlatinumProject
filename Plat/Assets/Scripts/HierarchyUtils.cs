using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class HierarchyUtils
    {
        public static List<T> GetComponentsInDirectChildren<T>(Transform parent,bool needActive)
            where T : Component
        {
            List<T> Tlist = new List<T>();
            for (int i = 0; i < parent.childCount; i++)
            {
                if(parent.GetChild(i).GetComponent<T>())
                {
                    if (needActive)
                    {
                        if (parent.GetChild(i).gameObject.activeInHierarchy)
                            Tlist.Add(parent.GetChild(i).GetComponent<T>());
                    }
                    else
                    {
                        Tlist.Add(parent.GetChild(i).GetComponent<T>());
                    }
                }
            }
            return Tlist;
        }

        public static T GetComponentInDirectChildren<T>(Transform parent, bool needActive)
            where T: Component
        {
            foreach (Transform item in parent)
            {
                if (!needActive || item.gameObject.activeSelf)
                {
                    T component = item.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
            return null;
        }

        public static T GetComponentUsingName<T>(Transform parent , string name)
            where T : Component
        {
            T value = null;
            foreach (Transform item in parent)
            {
                if (item.name == name)
                    return item.GetComponent<T>();
                else if (item.childCount > 0)
                {
                    value = GetComponentUsingName<T>(item, name);
                    if (value != null)
                        return value;
                }
            }
            return null;
        }
    }
}

