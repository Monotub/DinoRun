using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] Transform spawnLocation;

    public static GameObject lastChunkSpawned { get; private set; }

    private void Start()
    {
        lastChunkSpawned = ObjectPool.SharedInstance.pooledPrefabs[0];
        StartCoroutine(SpawnChunkOnDelay());
    }

    IEnumerator SpawnChunkOnDelay()
    {
        while (true)
        {
            GameObject chunk = ObjectPool.SharedInstance.GetPooledChunk();
            if (chunk)
            {
                chunk.transform.position = spawnLocation.position;
                chunk.transform.rotation = Quaternion.identity;
                chunk.transform.parent = transform;
                lastChunkSpawned = chunk;
                //Debug.Log($"Last Chunk Spawned: {lastChunkSpawned.name}, {lastChunkSpawned.GetComponent<Chunk>().HasHazard}");
                chunk.SetActive(true);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
