using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToPool;
    [SerializeField] int copiesOfPrefabs = 3;
    public static ObjectPool SharedInstance { get; private set; }
    public List<GameObject> pooledPrefabs = new List<GameObject>();

    Chunk lastChunk;
    //float defaultChunkSpd = 5f;       // Use this if setup to not reload scene 

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        // Populates object pool
        foreach (var chunk in prefabsToPool)
        {
            for (int i = 0; i < copiesOfPrefabs; i++)
            {
                GameObject tmp;
                tmp = Instantiate(chunk);
                tmp.transform.parent = transform;
                tmp.SetActive(false);
                pooledPrefabs.Add(tmp);
            }
        }
    }

    public GameObject GetPooledChunk()
    {
        lastChunk = ChunkSpawner.lastChunkSpawned.GetComponent<Chunk>();
        
        int rand = Random.Range(0, pooledPrefabs.Count);

        if (pooledPrefabs[rand] && !pooledPrefabs[rand].activeInHierarchy)
        {
            if(lastChunk.HasHazard && pooledPrefabs[rand].GetComponent<Chunk>().HasHazard)
            {
                // Need to change this. Recursion is bad.
                for (int i = 0; i < pooledPrefabs.Count; i++)
                {
                    if (!pooledPrefabs[i].GetComponent<Chunk>().HasHazard && !pooledPrefabs[i].activeInHierarchy)
                    {
                        return pooledPrefabs[i];
                    }
                }
            }
            else
                return pooledPrefabs[rand];
        }
        else
        {
            foreach (var chunk in pooledPrefabs)
            {
                if (!chunk.activeInHierarchy)
                    return chunk;
            }
        }
        return null;
    }

    public void SetAllChunkSpeed(float value)
    {
        for (int i = 0; i < pooledPrefabs.Count; i++)
        {
            pooledPrefabs[i].GetComponent<Chunk>().speed = value;
        }
    }
}
