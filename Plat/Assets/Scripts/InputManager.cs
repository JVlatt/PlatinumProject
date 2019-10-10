using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
public class InputManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GetManager()._inputManager = this;
    }
}
