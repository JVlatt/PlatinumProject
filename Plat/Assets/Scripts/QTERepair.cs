using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.UI;

public class QTERepair : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = true;

    private List<QTEKey> _keys = new List<QTEKey>();
    private List<QTEKey> _activeKeys = new List<QTEKey>();
    private Peon _peon;
    private GameObject middle;
    private ParticleSystem particle;

    public Transform topLeft;
    public Transform bottomRight;
    public Image Timer;
    private void Awake()
    {
        _keys = HierarchyUtils.GetComponentsInDirectChildren<QTEKey>(transform,false);
        middle = transform.GetChild(0).gameObject;
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Stop();
    }
    private void Start()
    {
        foreach (QTEKey k in _keys)
            k.gameObject.SetActive(false);
    }
    public void Launch(Peon peon)
    {
        peon.fixUpdate += UpdateTimer;
        _peon = peon;
        middle.SetActive(true);
        Timer.gameObject.SetActive(true);
        _activeKeys.Clear();
        switch (peon.name)
        {
            case "Oni":
                foreach(QTEKey k in _keys)
                    _activeKeys.Add(k);
                break;
            case "Naru":
                foreach (QTEKey k in _keys)
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

    private void UpdateTimer(float time)
    {
        Timer.fillAmount = time;
    }

    public void CheckEnd()
    {
        particle.Play();
        if(!_activeKeys.Find(x => !x.valid))
        {
            _peon.EndFix(true);
            Reset();
        }
    }

    public void Reset()
    {
        _peon.fixUpdate -= UpdateTimer;
        foreach (QTEKey k in _keys)
        {
            k.Reset();
            k.valid = false;
            k.gameObject.SetActive(false);
        }
        middle.SetActive(false);
        Timer.gameObject.SetActive(false);
        isActive = false;
    }

    public void TestLaunch()
    {
        Launch(PeonManager.Instance._peons.Find(x => x.name == "Taon"));
    }
}
