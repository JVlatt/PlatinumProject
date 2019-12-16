using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Script;
using UnityEngine.SceneManagement;

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
        if (_instance != null)
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
    private List<TextMeshProUGUI> _carriagesTags = new List<TextMeshProUGUI>();
    [SerializeField]
    private TextMeshProUGUI _nameTagPrefab;
    [SerializeField]
    private GameObject _attack;
    private Image _leftAttack;
    private Image _rightAttack;
    [SerializeField]
    private Image _blackScreen;
    [SerializeField]
    private Image _deathScreen;
    [SerializeField]
    private float _fadeSpeed;
    [SerializeField]
    private Sprite _boutonPersoSelect;
    [SerializeField]
    private Sprite _boutonPersoNone;
    float _timer;
    bool _fade;

    [Header("UI Perso")]
    [SerializeField]
    private List<Sprite> _classeState;
    [SerializeField]
    private List<Sprite> _healthState;
    [SerializeField]
    private List<Sprite> _activity;
    private List<UIInfoPerso> _UIInfoPersos = new List<UIInfoPerso>();
    [SerializeField]
    private List<PersoImage> _persosTetes;
    private Dictionary<string, Sprite> _dictPersoTete = new Dictionary<string, Sprite>();
    [SerializeField]
    private List<PersoImage> _persosName;
    private Dictionary<string, Sprite> _dictPersoName = new Dictionary<string, Sprite>();

    [SerializeField]
    private Transform _UIPeonPrefab;
    [SerializeField]
    private Transform _UIPeonConteneur;

    [Header("Cursor")]
    [SerializeField]
    private List<CursorClass> cursorClasses = new List<CursorClass>();
    private Dictionary<string, CursorClass> dictCursor = new Dictionary<string, CursorClass>();
    private CursorClass currentCursor;

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
    private float _characterDisplayTimer = 0f;
    private int _characterIndex;
    private string _textToWrite;
    private bool _textCompleted;
    [SerializeField]
    private Image _textTete;
    [SerializeField]
    private Image _textName;



    [Header("Choices Pannel")]
    [SerializeField]
    GameObject _choicePannel;

    public GameObject _thirdChoiceButton;

    private Peon _speakingCharacter;
    public Text _validText;
    public Text _cancelText;
    public Text _thirdText;

    public GameObject choicePannel
    {
        get { return _choicePannel; }
    }
    [SerializeField]
    private Text _choiceText;
    public string choiceText
    {
        set { _choiceText.text = value; }
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
    class PersoImage
    {
        public string name;
        public Sprite image;
    }


    [System.Serializable]
    class UIInfoPerso
    {
        public GameObject conteneur;
        public Image jobImage;
        public Image activityImage;
        public Image healthBar;
        public Image moralBar;
        public Image persoImage;
        public Button button;
    }

    [System.Serializable]
    class CursorClass
    {
        public string name;
        public List<Texture2D> textures = new List<Texture2D>();
        public float frameTime;
        [HideInInspector]
        public int currentTextureID=-1;
    }

    public enum FADETYPE
    {
        NULL,
        END,
        ADDCARRIAGE,
        FADEEVENT,
        ENDFADEEVENT,
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

    [HideInInspector]
    public bool textInstant;

    private int voiceId;
    private PhaseText.emotionType emotionType;

    public void UpdateMentalBar()
    {
        float total = 0;
        foreach (Peon peon in PeonManager.Instance._peons)
        {
            total += peon._mentalHealth;
        }
        _totalMentalHealth = (total / PeonManager.Instance._peons.Count) / 100;
    }

    public void UpdateUIPeon(Peon.PeonInfo peonInfo, int id)
    {
        if(peonInfo == null) { _UIInfoPersos[id].conteneur.SetActive(false); return; }
        _UIInfoPersos[id].healthBar.fillAmount = peonInfo.HP / peonInfo.HPMax;
        switch (peonInfo.ACTIVITY)
        {
            case Peon.ACTIVITY.NONE:
                _UIInfoPersos[id].activityImage.sprite = _activity[0];
                break;
            case Peon.ACTIVITY.HEAL:
                _UIInfoPersos[id].activityImage.sprite = _activity[1];
                break;
            case Peon.ACTIVITY.WAIT:
                _UIInfoPersos[id].activityImage.sprite = _activity[2];
                break;
            case Peon.ACTIVITY.FIGHT:
                if (peonInfo.TYPE == Peon.TYPE.FIGHTER)
                    _UIInfoPersos[id].activityImage.sprite = _activity[3];
                else
                    _UIInfoPersos[id].activityImage.sprite = _activity[4];
                break;
            case Peon.ACTIVITY.REPAIR:
                if (peonInfo.TYPE == Peon.TYPE.MECA)
                    _UIInfoPersos[id].activityImage.sprite = _activity[5];
                else
                    _UIInfoPersos[id].activityImage.sprite = _activity[6];
                break;
            case Peon.ACTIVITY.DRIVE:
                _UIInfoPersos[id].activityImage.sprite = _activity[7];
                break;
            case Peon.ACTIVITY.UNCLIP:
                _UIInfoPersos[id].activityImage.sprite = _activity[8];
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        foreach (var item in cursorClasses)
        {
            dictCursor[item.name] = item;
        }
        _leftAttack = _attack.transform.GetChild(0).GetComponent<Image>();
        _rightAttack = _attack.transform.GetChild(1).GetComponent<Image>();
        foreach (PersoImage item in _persosTetes)
        {
            _dictPersoTete.Add(item.name, item.image);
        }
        foreach (PersoImage item in _persosName)
        {
            _dictPersoName.Add(item.name, item.image);
        }
        SetupUIPerso();
    }

    private void SetupUIPerso()
    {
        for (int i = 0; i < 3; i++)
        {
            UIInfoPerso infoPerso = new UIInfoPerso();
            infoPerso.conteneur = _UIPeonConteneur.GetChild(i).gameObject;
            infoPerso.activityImage = HierarchyUtils.GetComponentUsingName<Image>(infoPerso.conteneur.transform, "ActivityImage");
            infoPerso.healthBar = HierarchyUtils.GetComponentUsingName<Image>(infoPerso.conteneur.transform, "HealthBar");
            infoPerso.jobImage = HierarchyUtils.GetComponentUsingName<Image>(infoPerso.conteneur.transform, "JobImage");
            infoPerso.moralBar = HierarchyUtils.GetComponentUsingName<Image>(infoPerso.conteneur.transform, "MoralBar");
            infoPerso.persoImage = HierarchyUtils.GetComponentUsingName<Image>(infoPerso.conteneur.transform, "Tete");
            infoPerso.button = HierarchyUtils.GetComponentUsingName<Button>(infoPerso.conteneur.transform, "Bouton");
            _UIInfoPersos.Add(infoPerso);
        }
    }

    private void Update()
    {

        if (_fade && _fadeType != FADETYPE.NULL)
        {
            _timer += Time.deltaTime;
            _blackScreen.color = new Color(0, 0, 0, _timer * _fadeSpeed);
            if (_timer * _fadeSpeed > 1)
            {
                if (_fadeType != FADETYPE.FADEEVENT)
                {
                    _fade = false;
                    if (_fadeType == FADETYPE.ADDCARRIAGE)
                        TrainManager.Instance.AddCarriage();
                    if (_fadeType == FADETYPE.END)
                        _deathScreen.color = new Color(1, 1, 1, _timer * _fadeSpeed);
                }
                else
                {
                    PhaseManager.Instance.NextPhase();
                    _fadeType = FADETYPE.NULL;
                }
            }
        }
        else if (_fadeType != FADETYPE.NULL && _fadeType!=FADETYPE.END)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _blackScreen.color = new Color(0, 0, 0, _timer * _fadeSpeed);
            }
            else
            {
                if (_blackScreen.gameObject.activeSelf)
                    _blackScreen.gameObject.SetActive(false);
                if (_fadeType == FADETYPE.ENDFADEEVENT)
                {
                    PhaseManager.Instance.NextPhase();
                    _fadeType = FADETYPE.NULL;

                }
            }
        }

        for (int i = 0; i < _UIPeons.Count; i++)
        {
            if (i < PeonManager.Instance._peons.Count && PeonManager.Instance._peons[i] != null)
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
            {
                if (!_textCompleted)
                {
                    _textCompleted = true;
                    _text.SetText(_textToWrite);
                }
                else
                    _textDisplayTimer = _textDisplayDuration;
            }

            _textDisplayTimer += Time.deltaTime;
            _characterDisplayTimer += Time.deltaTime;
            if (_characterDisplayTimer >= 0.01f && !_textCompleted)
            {
                if (_characterIndex < _textToWrite.Length)
                    _characterIndex++;
                else
                    _textCompleted = true;
                _characterDisplayTimer = 0f;
                _text.SetText(_textToWrite.Substring(0, _characterIndex));

            }
            if (_textDisplayTimer >= _textDisplayDuration - 0.5f)
            {
                switch (emotionType)
                {
                    case PhaseText.emotionType.NORMAL:
                        if (_speakingCharacter && SoundManager.Instance.isPlaying(_speakingCharacter.name + voiceId))
                        {
                            SoundManager.Instance.StopSound(_speakingCharacter.name + voiceId);
                        }
                        break;
                    case PhaseText.emotionType.SAD:
                        if (_speakingCharacter && SoundManager.Instance.isPlaying("sad" + _speakingCharacter.name))
                        {
                            SoundManager.Instance.StopSound("sad" + _speakingCharacter.name);
                        }
                        break;
                    case PhaseText.emotionType.ANGRY:
                        if (_speakingCharacter && SoundManager.Instance.isPlaying("angry" + _speakingCharacter.name))
                        {
                            SoundManager.Instance.StopSound("angry" + _speakingCharacter.name);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (_textDisplayTimer >= _textDisplayDuration)
            {
                _textDisplayTimer = 0f;
                if (!textInstant)
                {
                    Phase nextPhase = PhaseManager.Instance.GetNextPhase();
                    if (nextPhase != null && nextPhase.KillTextBox())
                    {
                        _text.SetText(" ");
                        _textPannel.SetActive(false);
                    }
                    if (_speakingCharacter)
                        _speakingCharacter.isTalking = false;
                    PhaseManager.Instance.NextPhase();
                }
                else
                {
                    _text.SetText(" ");
                    _textPannel.SetActive(false);
                    if (_speakingCharacter)
                        _speakingCharacter.isTalking = false;
                }

            }
        }
        CursorUpdate();
    }

    private Color ResetColor(Color color)
    {
        color.a = 0;
        return color;
    }

    public void AddUIPeon(Peon p)
    {
        Transform UI;
        if (p._ID >= _UIPeons.Count)
        {
            UI = Instantiate(_UIPeonPrefab);
            _UIPeons.Add(UI);
        }
        else
            UI = _UIPeons[p._ID];
        UIInfoPerso uiInfoPerso = _UIInfoPersos[p._ID];
        uiInfoPerso.conteneur.SetActive(true);

        uiInfoPerso.button.onClick.RemoveAllListeners();
        uiInfoPerso.button.onClick.AddListener(() => SelectPerso(p));

        UI.SetParent(transform);

        GameObject fix = UI.GetChild(0).gameObject;
        p._fix = fix;



        switch (p._peonInfo.TYPE)
        {
            case Peon.TYPE.HEALER:
                uiInfoPerso.jobImage.sprite = _classeState[0];
                break;
            case Peon.TYPE.MECA:
                uiInfoPerso.jobImage.sprite = _classeState[1];
                break;
            case Peon.TYPE.SIMPLE:
                uiInfoPerso.jobImage.sprite = _classeState[2];
                break;
            case Peon.TYPE.FIGHTER:
                uiInfoPerso.jobImage.sprite = _classeState[3];
                break;
        }
        uiInfoPerso.persoImage.sprite = _dictPersoTete[p.name];
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
        if (currentCursor == dictCursor[type]) return;
        if (dictCursor.ContainsKey(type))
        {
            if(currentCursor!=null)
                currentCursor.currentTextureID = -1;
            currentCursor = dictCursor[type];
        }
        else
            Debug.LogError("Cursor : " + type + " not set");
    }

    private void CursorUpdate()
    {
        if (currentCursor == null) return;
        if (currentCursor.textures.Count != 1)
        {
            float time = (Time.time % (currentCursor.textures.Count*currentCursor.frameTime));
            float i = 0;
            int iD = -1;
            while (i < time)
            {
                iD++;
                i += currentCursor.frameTime;
            }
            if (iD != currentCursor.currentTextureID)
            {
                Cursor.SetCursor(currentCursor.textures[iD], Vector2.zero, CursorMode.Auto);
                currentCursor.currentTextureID = iD;
            }
        }
        else
        {
            if (currentCursor.currentTextureID != 0)
            {
                Cursor.SetCursor(currentCursor.textures[0], Vector2.zero, CursorMode.Auto);
                currentCursor.currentTextureID = 0;
            }
        }
    }

    public void DisplayText(string text, string character, float duration, bool instant, PhaseText.emotionType emotion)
    {
        textInstant = instant;
        _textCompleted = false;
        _characterIndex = 0;
        _textToWrite = text;
        _speakingCharacter = PeonManager.Instance._peons.Find(x => character == x.name);
        _speakingCharacter.isTalking = true;
        _textTete.sprite = _dictPersoTete[character];
        _textName.sprite = _dictPersoName[character];
        emotionType = emotion;
        switch (emotionType)
        {
            case PhaseText.emotionType.NORMAL:
                voiceId = Random.Range(1, 4);
                SoundManager.Instance.Play(character + voiceId);
                break;
            case PhaseText.emotionType.SAD:
                SoundManager.Instance.Play("sad" + character);
                break;
            case PhaseText.emotionType.ANGRY:
                SoundManager.Instance.Play("angry" + character);
                break;
            default:
                break;
        }

        _textDisplayDuration = duration;
        _textPannel.SetActive(true);
    }

    public void SelectPerso(bool active, int ID)
    {
        if (active)
            _UIInfoPersos[ID].button.image.sprite = _boutonPersoSelect;
        else
            _UIInfoPersos[ID].button.image.sprite = _boutonPersoNone;
    }

    public void Fade(FADETYPE fadeType)
    {
        _fadeType = fadeType;
        if (fadeType == FADETYPE.ENDFADEEVENT)
            _fade = false;
        else
            _fade = true;
        _blackScreen.gameObject.SetActive(true);
    }

    public void SelectPerso(Peon peon)
    {
        if (peon.CanBeSelect())
            PeonManager.Instance._activePeon = peon;
    }

    public void PassText()
    {
        if (!_textCompleted)
        {
            _textCompleted = true;
            _text.SetText(_textToWrite);
        }
        else
            _textDisplayTimer = _textDisplayDuration;
    }

    public void NextPhase()
    {
        if (PhaseManager.Instance.activePhase.GetPhaseType() == Phase.PhaseType.WAITTOUCH)
            PhaseManager.Instance.NextPhase();
    }

    public void EndChoice(bool yes)
    {
        PhaseManager.Instance.EndCondition(yes);
        _choicePannel.SetActive(false);
    }

    public void BoutonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BoutonQuit()
    {
        Application.Quit();
    }
}
