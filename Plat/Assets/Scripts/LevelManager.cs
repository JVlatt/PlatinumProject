using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GetManager()._levelManager = this;
    }

    private void Start()
    {
        Debug.Log(GameManager.GetManager()._soundManager.gameObject.name);
    }
}
