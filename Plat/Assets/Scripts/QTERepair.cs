using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class QTERepair : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = true;

    private List<QTEKey> _keys = new List<QTEKey>();
    private List<QTEKey> _activeKeys = new List<QTEKey>();
    private Peon _peon;
    private GameObject middle;
    private void Awake()
    {
        _keys = HierarchyUtils.GetComponentInDirectChildren<QTEKey>(transform,false);
        middle = transform.GetChild(0).gameObject;
    }
    public void Launch(Peon peon)
    {
        _peon = peon;
        middle.SetActive(true);
        _activeKeys.Clear();
        switch (peon.name)
        {
            case "Oni":
                foreach(QTEKey k in _keys)
                    _activeKeys.Add(k);
                break;
            case "Taon":
                _activeKeys.Add(_keys[1]);
                _activeKeys.Add(_keys[3]);
                _activeKeys.Add(_keys[5]);
                _activeKeys.Add(_keys[7]);
                break;
            case "Butor":
                _activeKeys.Add(_keys[0]);
                _activeKeys.Add(_keys[1]);
                _activeKeys.Add(_keys[3]);
                _activeKeys.Add(_keys[4]);
                _activeKeys.Add(_keys[5]);
                _activeKeys.Add(_keys[7]);
                break;
        }
        foreach (QTEKey k in _activeKeys)
            k.gameObject.SetActive(true);
        isActive = true;
    }

    public void CheckEnd()
    {
        if(!_activeKeys.Find(x => !x.valid))
        {
            Reset();
        }
    }

    public void Reset()
    {
        foreach (QTEKey k in _keys)
        {
            k.transform.position = k.startPosition;
            k.valid = false;
            k.gameObject.SetActive(false);
        }
        middle.SetActive(false);
        isActive = false;
    }

    public void TestLaunch()
    {
        Launch(PeonManager.Instance._peons.Find(x => x.name == "Taon"));
    }
}
