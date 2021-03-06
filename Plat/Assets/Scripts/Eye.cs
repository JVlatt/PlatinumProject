﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    QTEScript _qte;
    private bool _isOpen;
    public bool isOpen
    {
        get { return _isOpen; }
        set
        {
            if(value == true)
            {
                myAnim.SetTrigger("Open");
            }
            else
            {
                if (_duration > 0)
                {
                    myAnim.SetTrigger("Death");
                    particle.Play();
                }
                else
                    myAnim.SetTrigger("Close");
            }
            _isOpen = value;
        }
    }
    private float _duration;
    private Animator myAnim;
    private ParticleSystem particle;
    private bool _blockTimer;
    private void Awake()
    {
        _qte = GetComponentInParent<QTEScript>();
        myAnim = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Stop();
        gameObject.SetActive(false);
    }
    public void SpawnEye(float duration)
    {
        gameObject.SetActive(true);
        particle.Stop();
        if (duration > 0)
        {
            _duration = duration;
            isOpen = true;
            _blockTimer = false;
        }
        else
        {
            _duration = 10;
            isOpen = true;
            _blockTimer = true;
        }
    }
    void Update()
    {
        if(_isOpen && !_blockTimer)
        {
            myAnim.SetTrigger("Open");
            _duration -= Time.deltaTime;
            if (_duration <= 0)
            {
                isOpen = false;
            }
        }
    }
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if(_isOpen)
            {
                _qte.point++;
                isOpen = false;
                _qte.CheckEnd();
                int i = Random.Range(0,3);
                SoundManager.Instance.Play("Plop " + i);
            }
        }
    }

    private void OnMouseEnter()
    {
        UIManager.Instance.ChangeCursor("target");
    }

    private void OnMouseExit()
    {
        UIManager.Instance.ChangeCursor("default");
    }


    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
