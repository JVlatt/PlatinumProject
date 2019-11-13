using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    #region Singleton
    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [Header("Mental")]
    public Image _mentalBar;
    private float m_totalMentalHealth;

    [Header("UI")]
    [SerializeField]
    private Vector3 _nameTagOffset;
    private List<Transform> _UIPeons = new List<Transform>();
    private List<Image> _lifeBars = new List<Image>();
    private List<TextMeshProUGUI> _nameTags = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _carriagesTags = new List<TextMeshProUGUI>();
    [SerializeField]
    private TextMeshProUGUI _nameTagPrefab;
    [SerializeField]
    private Transform _UIPeonPrefab;
    [SerializeField]
    private GameObject _attack;
    private Image _leftAttack;
    private Image _rightAttack;
    [SerializeField]
    private Image _blackScreen;
    [SerializeField]
    private float _fadeSpeed;
    float _timer;
    bool _fade;

    [Header("UI Perso")]
    [SerializeField]
    private List<Sprite> _classeState;
    [SerializeField]
    private List<Sprite> _healthState;
    [SerializeField]
    private UIInfoPerso _UIInfoPerso;
    [SerializeField]
    private LineRenderer _line;


    [Header("Cursor")]
    [SerializeField]
    private Texture2D _cursorDefault;
    [SerializeField]
    private Texture2D _cursorPeon;
    [SerializeField]
    private Texture2D _cursorFix;
    [SerializeField]
    private Texture2D _cursorAttack;

    [Header("Debug")]
    [SerializeField]
    private bool m_isOnAttack;
    private float _timerAttack;

    [Header("Text Pannel")]
    [SerializeField]
    private GameObject _textPannel;
    [SerializeField]
    private TextMeshProUGUI _text;
    private float _textDisplayTimer = 0f;
    private float _textDisplayDuration = 0f;
    [SerializeField]
    private Image _butorImg;
    [SerializeField]
    private Image _taonImg;
    [SerializeField]
    private Image _oniImg;

    [Header("Choices Pannel")]
    [SerializeField]
    GameObject _choicePannel;
    public GameObject choicePannel
    {
        get { return _choicePannel; }
    }
    [SerializeField]
    Image _choiceClock;
    public Image choiceClock
    {
        get { return _choiceClock; }
    }

    private FADETYPE _fadeType;

    #region Struct
    [System.Serializable]
    class UIInfoPerso
    {
        public GameObject conteneur;
        public Text name;
        public Text typeText;
        public Text heathStateText;
        public Image visual;
        public Image typeImage;
        public Image healthStateImage;
        public Image healthBar;
        public Image moralBar;
    }

    public enum FADETYPE
    {
        NULL,
        END,
        ADDCARRIAGE
    }
    #endregion

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

    

    public void UpdateMentalBar()
    {
        float total = 0;
        foreach (Peon peon in PeonManager.Instance._peons)
        {
            total += peon._mentalHealth;
        }
        _totalMentalHealth = (total / PeonManager.Instance._peons.Count) / 100;
    }

    public void UpdateHealthBar(float amont, int ID)
    {
        _lifeBars[ID].fillAmount = amont;
    }

    public void UpdateUIPeon(Peon.PeonInfo peonInfo)
    {
        if (peonInfo == null)
        {
            _UIInfoPerso.conteneur.SetActive(false);
            _line.gameObject.SetActive(false);
        }
        else
        {
            _UIInfoPerso.visual.sprite = peonInfo.visual;
            _UIInfoPerso.healthBar.fillAmount = peonInfo.HP / peonInfo.HPMax;
            _UIInfoPerso.name.text = peonInfo.name;
            switch (peonInfo.HEALTHSTATE)
            {
                case Peon.HEALTHSTATE.HURT:

                    _UIInfoPerso.healthStateImage.sprite = _healthState[0];
                    _UIInfoPerso.heathStateText.text = "Hurt";
                    break;
                case Peon.HEALTHSTATE.TREAT:
                    _UIInfoPerso.healthStateImage.sprite = _healthState[1];
                    _UIInfoPerso.heathStateText.text = "Treat";
                    break;
                case Peon.HEALTHSTATE.GOOD:
                    _UIInfoPerso.healthStateImage.sprite = _healthState[2];
                    _UIInfoPerso.heathStateText.text = "Good";
                    break;
                default:
                    break;
            }
            switch (peonInfo.TYPE)
            {
                case Peon.TYPE.HEALER:
                    _UIInfoPerso.typeImage.sprite = _classeState[0];
                    _UIInfoPerso.typeText.text = "Healer";
                    break;
                case Peon.TYPE.MECA:
                    _UIInfoPerso.typeImage.sprite = _classeState[1];
                    _UIInfoPerso.typeText.text = "Mecano";
                    break;
                case Peon.TYPE.SIMPLE:
                    _UIInfoPerso.typeImage.sprite = _classeState[2];
                    _UIInfoPerso.typeText.text = "Simple";
                    break;
                case Peon.TYPE.FIGHTER:
                    _UIInfoPerso.typeImage.sprite = _classeState[3];
                    _UIInfoPerso.typeText.text = "Fighter";
                    break;
                default:
                    break;
            }
            _UIInfoPerso.conteneur.SetActive(true);
            _line.gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        _leftAttack = _attack.transform.GetChild(0).GetComponent<Image>();
        _rightAttack = _attack.transform.GetChild(1).GetComponent<Image>();
    }

    private void Update()
    {
        if (PeonManager.Instance._activePeon)
        {
            Vector3 direction = Camera.main.WorldToScreenPoint(PeonManager.Instance._activePeon.transform.position)- Camera.main.WorldToScreenPoint(_line.transform.position);
            direction.z = 0;
            _line.SetPosition(2, (direction.normalized)*0.5f);
        }

        if (_fade)
        {
            _timer += Time.deltaTime;
            _blackScreen.color = new Color(0, 0, 0, _timer * _fadeSpeed);
            if (_timer * _fadeSpeed > 1.5)
            {
                _fade = false;
                if(_fadeType == FADETYPE.ADDCARRIAGE)
                    TrainManager.Instance.AddCarriage();
            }
        }
        else
        {
            _timer -= Time.deltaTime;
            _blackScreen.color = new Color(0, 0, 0, _timer * _fadeSpeed);
        }

        for (int i = 0; i < _UIPeons.Count; i++)
        {
            if(PeonManager.Instance._peons[i] != null)
            {
                Vector3 offSetPos = PeonManager.Instance._peons[i].transform.position;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(offSetPos);
                _UIPeons[i].position = screenPos;
            }
        }
        for (int i = 0; i < _carriagesTags.Count; i++)
        {
            Vector3 offSetPos = TrainManager.Instance._carriages[i].transform.position + _nameTagOffset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(offSetPos);
            _carriagesTags[i].transform.position = screenPos;
        }
        if (_isOnAttack)
        {
            _timerAttack += Time.deltaTime;
            Color color = _leftAttack.color;
            if (_timerAttack < 1)
                color.a = Mathf.Lerp(0, 1, _timerAttack);
            else if (_timerAttack < 2)
                color.a = Mathf.Lerp(0, 1, (1 - (_timerAttack - 1)));
            else
                _timerAttack = 0;
            TrainManager.AttackedCariageDirection direction = TrainManager.Instance.CheckAttackedCariageDirection();
            if (direction.Left)
                _leftAttack.color = color;
            else
                _leftAttack.color = ResetColor(_leftAttack.color);
            if (direction.Right)
                _rightAttack.color = color;
            else
                _rightAttack.color = ResetColor(_leftAttack.color);

        }
        else
        {
            _leftAttack.color = ResetColor(_leftAttack.color);
            _rightAttack.color = ResetColor(_leftAttack.color);
        }

        if (_textPannel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _textDisplayTimer = _textDisplayDuration;

            _textDisplayTimer += Time.deltaTime;
            if (_textDisplayTimer >= _textDisplayDuration)
            {
                _textDisplayTimer = 0f;
                Phase nextPhase = PhaseManager.Instance.GetNextPhase();
                if (nextPhase != null && nextPhase.KillTextBox())
                {
                    _text.SetText(" ");
                    _textPannel.SetActive(false);
                }
                
                PhaseManager.Instance.NextPhase();
            }
        }
    }

    private Color ResetColor(Color color)
    {
        color.a = 0;
        return color;
    }

    public void AddUIPeon(Peon p)
    {
        Transform UI = Instantiate(_UIPeonPrefab);
        _UIPeons.Add(UI);
        UI.SetParent(transform);
        GameObject over = UI.GetChild(0).gameObject;
        p._over = over;
        GameObject fix = UI.GetChild(1).gameObject;
        p._fix = fix;

        Image image = over.transform.GetChild(1).GetComponentInChildren<Image>();
        _lifeBars.Add(image);

        TextMeshProUGUI text = over.GetComponentInChildren<TextMeshProUGUI>();
        text.SetText(p._peonInfo.name.ToString());
        _nameTags.Add(text);
    }

    public void AddCarriageName(Carriage c)
    {
        TextMeshProUGUI text = Instantiate(_nameTagPrefab);
        text.SetText(c.transform.parent.gameObject.name);
        _carriagesTags.Add(text);
        text.transform.SetParent(transform);
        text.gameObject.SetActive(false);
        c.nameTag = text;
    }

    public void RemoveCarriageName(Carriage c)
    {
        _carriagesTags.Remove(c.nameTag);
    }

    public void ChangeCursor(string type)
    {
        switch (type)
        {
            case "default":
                Cursor.SetCursor(_cursorDefault, Vector2.zero, CursorMode.Auto);
                break;
            case "fix":
                Cursor.SetCursor(_cursorFix, Vector2.zero, CursorMode.Auto);
                break;
            case "peon":
                Cursor.SetCursor(_cursorPeon, Vector2.zero, CursorMode.Auto);
                break;
            case "attack":
                Cursor.SetCursor(_cursorAttack, Vector2.zero, CursorMode.Auto);
                break;
        }
    }

    public void DisplayText(string text, string character, float duration)
    {
        _oniImg.gameObject.SetActive(false);
        _taonImg.gameObject.SetActive(false);
        _butorImg.gameObject.SetActive(false);
        _text.SetText(text);

        switch (character)
        {
            case "Butor":
                _butorImg.gameObject.SetActive(true);
                break;
            case "Taon":
                _taonImg.gameObject.SetActive(true);
                break;
            case "Oni":
                _oniImg.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        SoundManager.Instance.Play(character);
        _textDisplayDuration = duration;
        _textPannel.SetActive(true);
    }

    public void fade(FADETYPE fadeType)
    {
        _fadeType = fadeType;
        _fade = true;
    }
}
