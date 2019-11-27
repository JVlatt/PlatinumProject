using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _availablesChunks = new List<GameObject>();
    [SerializeField]
    private List<Chunk> m_activeChunk = new List<Chunk>();
    [SerializeField]
    private float m_scrollSpeed = 1.0f;
    [SerializeField]
    private Transform m_resetPos;
    [SerializeField]
    private Transform m_spawnPos;

    private void Start()
    {
        foreach (GameObject g in _availablesChunks)
            g.SetActive(false);
    }
    private void Update()
    {
        foreach (Chunk chunk in m_activeChunk)
        {
            chunk.transform.Translate(new Vector3(-m_scrollSpeed * Time.deltaTime*(TrainManager.Instance.Speed/50), 0, 0));
        }
        if (m_activeChunk[0].endPosition.position.x <= m_resetPos.position.x)
        {
            _availablesChunks.Add(m_activeChunk[0].gameObject);
            m_activeChunk[0].gameObject.SetActive(false);
            m_activeChunk.RemoveAt(0);
        }
        if (m_activeChunk[m_activeChunk.Count - 1].endPosition.position.x <= m_spawnPos.position.x)
        {
            int random = Random.Range(0, _availablesChunks.Count - 1);
            _availablesChunks[random].transform.position = m_activeChunk[m_activeChunk.Count - 1].endPosition.position;
            _availablesChunks[random].gameObject.SetActive(true);
            m_activeChunk.Add(_availablesChunks[random].GetComponent<Chunk>());
            _availablesChunks.RemoveAt(random);
        }
    }
}
