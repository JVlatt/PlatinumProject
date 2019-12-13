using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChoice : Phase
{
    [Header("Phase Choice Parameters")]
    [SerializeField]
    float _choiceDuration;
    [TextArea]
    [SerializeField]
    private string _text;
    [SerializeField]
    ChoiceType _choice;

    [SerializeField]
    private string _validateText;
    [SerializeField]
    private string _cancelText;
    [SerializeField]
    private string _thirdText;

    private float _choiceTimer;
    private bool _isLaunched = false;
    

    public enum ChoiceType
    {
        UNCLIP,
        ONI,        
        NEWCARRIAGE,
        LASTCHOICE2,
        LASTCHOICE3,
    }

    private void Start()
    {
        type = PhaseType.CHOICE;
        freezeControl = true;
    }

    public override string BuildGameObjectName()
    {
        return "Choice";
    }

    public override void LaunchPhase()
    {
        UIManager.Instance.choiceText = _text;
        UIManager.Instance._validText.text = _validateText;
        UIManager.Instance._cancelText.text = _cancelText;
        if(_choice == ChoiceType.LASTCHOICE3)
        {
            UIManager.Instance._thirdText.text = _thirdText;
            UIManager.Instance._thirdChoiceButton.SetActive(true);
        }
        UIManager.Instance.choicePannel.SetActive(true);
        if (_choice != ChoiceType.LASTCHOICE2 && _choice != ChoiceType.LASTCHOICE3)
        {
            _isLaunched = true;
            _choiceTimer = 0;
        }
    }

    private void Update()
    {
        if (_isLaunched)
        {
            _choiceTimer += Time.deltaTime;
            UIManager.Instance.choiceClock.fillAmount = _choiceTimer / _choiceDuration;
            if (_choiceTimer > _choiceDuration)
            {
                UIManager.Instance.EndChoice(false);
                _isLaunched = false;
            }
            if (PhaseManager.Instance.activePhase != this)
                _isLaunched = false;
        }
    }
}
