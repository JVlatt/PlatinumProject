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
    [SerializeField]
    private Image _leftAttack;
    [SerializeField]
    private bool m_isOnAttack;
    private float _timerAttack;

    public float _totalMentalHealth
    {
        get { return m_totalMentalHealth; }
        set
        {
            m_totalMentalHealth = value;
            _mentalBar.fillAmount = value;
        }
    }
    public bool _isOnAttack
    {
        get { return m_isOnAttack; }
        set { m_isOnAttack = value; }
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
        if (_isOnAttack)
        {
            _timerAttack += Time.deltaTime;
            Color color = _leftAttack.color;
            if (_timerAttack < 1)
                color.a = Mathf.Lerp(0, 0.2f, _timerAttack );
            else if (_timerAttack < 2)
                color.a = Mathf.Lerp(0, 0.2f, (1 - (_timerAttack - 1)) );
            else
                _timerAttack = 0;
            _leftAttack.color = color;

        }
        else
        {
            Color color = _leftAttack.color;
            color.a = 0;
            _leftAttack.color = color;
        }
    }

    public void AddLifeBar()
    {
        Image image = Instantiate(_lifeBarPrefab);
        _lifeBars.Add(image);
        image.transform.SetParent( transform);
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
