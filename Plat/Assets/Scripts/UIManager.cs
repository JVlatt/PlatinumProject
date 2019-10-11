using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class UIManager : MonoBehaviour
{
    public Image _mentalBar;
    private float m_totalMentalHealth;
    public float _totalMentalHealth
    {
        get { return m_totalMentalHealth; }
        set
        {
            m_totalMentalHealth = value;
            _mentalBar.fillAmount = value;
        }
    }

    private void Awake()
    {
        GameManager.GetManager()._UIManager = this;
        UpdateMentalBar();
    }

    public void UpdateMentalBar()
    {
        float total = 0;
        foreach (Peon peon in GameManager.GetManager()._peonManager._peons)
        {
            total += peon._mentalHealth;
        }
        m_totalMentalHealth = (total / GameManager.GetManager()._peonManager._peons.Count)/100;
    }
}
