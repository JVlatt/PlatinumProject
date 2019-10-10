using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField]
    private Transform m_startPosition;
    public Transform startPosition
    {
        get { return m_startPosition; }
        set { m_startPosition = value; }
    }
    [SerializeField]
    private Transform m_endPosition;
    public Transform endPosition
    {
        get { return m_endPosition; }
        set { m_endPosition = value; }
    }
}
