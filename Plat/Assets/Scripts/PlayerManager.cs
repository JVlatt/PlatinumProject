using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PlayerManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GetManager()._players = this;
    }
}
