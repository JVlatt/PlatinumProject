using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_chunkPrefabs = new List<GameObject>();
    [SerializeField]
    private List<Chunk> m_activeChunk = new List<Chunk>();
    [SerializeField]
    private float m_scrollSpeed = 1.0f;
    [SerializeField]
    private Transform m_resetPos;
    [SerializeField]
    private Transform m_spawnPos;

    private void Update()
    {
        foreach (Chunk chunk in m_activeChunk)
        {
            chunk.transform.Translate(new Vector3(-m_scrollSpeed * Time.deltaTime, 0, 0));
        }
        if (m_activeChunk[0].endPosition.position.x <= m_resetPos.position.x)
        {
            Destroy(m_activeChunk[0].gameObject);
            m_activeChunk.RemoveAt(0);
        }
        if (m_activeChunk[m_activeChunk.Count - 1].endPosition.position.x <= m_spawnPos.position.x)
        {
            int random = Random.Range(0, m_chunkPrefabs.Count - 1);
            m_activeChunk.Add(Instantiate(m_chunkPrefabs[random], m_activeChunk[m_activeChunk.Count - 1].endPosition.position, Quaternion.identity, transform).GetComponent<Chunk>());
        }
    }
}
