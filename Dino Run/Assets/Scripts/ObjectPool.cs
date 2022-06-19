using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToPool;
    [SerializeField] int copiesOfPrefabs = 3;
    public static ObjectPool Instance { get; private set; }
    public List<GameObject> pooledPrefabs = new List<GameObject>();
    public int chunksSinceLastHazard { get; private set; }
    public int chunkSpeed = 5;

    Chunk lastChunk;

    private void Awake()
    {
        Instance = this;
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

        // Returns Hazard chunk if there's too much dead space
        if(chunksSinceLastHazard >= 2)
        {
            foreach (var chunk in pooledPrefabs)
            {
                if (chunk.GetComponent<Chunk>().HasHazard && !chunk.activeInHierarchy)
                {
                    chunksSinceLastHazard = 0;
                    return chunk;
                }
            }
        }

        if (pooledPrefabs[rand] && !pooledPrefabs[rand].activeInHierarchy)
        {
            MonitorHazards(rand);
            return pooledPrefabs[rand];
            // Ensures 2 hazards don't spawn next to each other
            //if(lastChunk.HasHazard && pooledPrefabs[rand].GetComponent<Chunk>().HasHazard)
            //{
            //    for (int i = 0; i < pooledPrefabs.Count; i++)
            //    {
            //        if (!pooledPrefabs[i].GetComponent<Chunk>().HasHazard && !pooledPrefabs[i].activeInHierarchy)
            //        {
            //            MonitorHazards(i);
            //            return pooledPrefabs[i];
            //        }
            //    }
            //}
            //else
            //{
            //    MonitorHazards(rand);
            //    return pooledPrefabs[rand];
            //}
        }
        else
        {
            // Ensures there are no empty spots
            for (int i = 0; i < pooledPrefabs.Count; i++)
            {
                if (!pooledPrefabs[i].activeInHierarchy)
                {
                    MonitorHazards(i);
                    return pooledPrefabs[i];
                }
            }
        }
        
        return null;
    }

    private void MonitorHazards(int i)
    {
        if (!pooledPrefabs[i].GetComponent<Chunk>().HasHazard) chunksSinceLastHazard++;
        else chunksSinceLastHazard = 0;
    }
}
