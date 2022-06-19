using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] Transform spawnLocation;
    
    public static GameObject lastChunkSpawned { get; private set; }

    bool spawnStarted = false;


    private void Start() => lastChunkSpawned = ObjectPool.Instance.pooledPrefabs[0];

    private void Update()
    {
        if (spawnStarted || GameManager.Instance.gamePaused) return;

        spawnStarted = true;
        StartCoroutine(SpawnChunkOnDelay());
    }

    IEnumerator SpawnChunkOnDelay()
    {
        while (true)
        {
            GameObject chunk = ObjectPool.Instance.GetPooledChunk();
            if (chunk)
            {
                chunk.transform.position = spawnLocation.position;
                chunk.transform.rotation = Quaternion.identity;
                chunk.transform.parent = transform;
                lastChunkSpawned = chunk;
                chunk.SetActive(true);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public void ChangeSpawnSpeed(float value)
    {
        StopAllCoroutines();
        spawnDelay -= value;
        StartCoroutine(SpawnChunkOnDelay());
    }
}
