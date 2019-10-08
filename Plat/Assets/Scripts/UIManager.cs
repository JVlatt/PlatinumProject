using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GetManager()._UIManager = this;
    }
}
