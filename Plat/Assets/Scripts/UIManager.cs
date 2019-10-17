using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class UIManager : MonoBehaviour
{
    public Image _mentalBar;
    private float m_totalMentalHealth;
    private List<Image> _lifeBars = new List<Image>();
    [SerializeField]
    private Image _lifeBarPrefab;
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
    }

    public void UpdateMentalBar()
    {
        float total = 0;
        foreach (Peon peon in GameManager.GetManager()._peonManager._peons)
        {
            total += peon._mentalHealth;
        }
        _totalMentalHealth = (total / GameManager.GetManager()._peonManager._peons.Count)/100;
    }

    public void UpdateHealthBar(float amont,int ID)
    {
        _lifeBars[ID].fillAmount = amont;
    }

    private void Update()
    {
        for (int i = 0; i < _lifeBars.Count; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.GetManager()._peonManager._peons[i].transform.position);
            _lifeBars[i].transform.position = screenPos; 
        }
    }

    public void AddLifeBar()
    {
        Image image = Instantiate(_lifeBarPrefab);
        _lifeBars.Add(image);
        image.transform.parent = transform;
    }
}
