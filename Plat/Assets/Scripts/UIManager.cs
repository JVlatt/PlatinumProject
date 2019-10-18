using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image _mentalBar;
    private float m_totalMentalHealth;
    private List<Image> _lifeBars = new List<Image>();
    private List<TextMeshProUGUI> _nameTags = new List<TextMeshProUGUI>();
    [SerializeField]
    private Image _lifeBarPrefab;
    [SerializeField]
    private TextMeshProUGUI _nameTagPrefab;
    [SerializeField]
    private Vector3 _nameTagOffset;

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
            Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.GetManager()._peonManager._peons[i].transform.position + _nameTagOffset);
            _lifeBars[i].transform.position = screenPos; 
        }
        for (int i = 0; i < _nameTags.Count; i++)
        {
            Vector3 offSetPos = GameManager.GetManager()._peonManager._peons[i].transform.position + _nameTagOffset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(offSetPos);
            _nameTags[i].transform.position = screenPos;
        }
    }

    public void AddLifeBar()
    {
        Image image = Instantiate(_lifeBarPrefab);
        _lifeBars.Add(image);
        image.transform.parent = transform;
    }

    public void AddNameTag(Peon p)
    {
        TextMeshProUGUI text = Instantiate(_nameTagPrefab);
        text.SetText(p._type.ToString());
        _nameTags.Add(text);
        text.transform.SetParent(transform);
        p.nameTag = text;
    }
}
