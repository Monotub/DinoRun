using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    [SerializeField] bool hasHazard = false;

    public bool HasHazard => hasHazard;
    
    float speed;


    void Update()
    {
        if (GameManager.Instance.gamePaused) return;

        speed = ObjectPool.Instance.chunkSpeed;
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChunkKiller"))
            gameObject.SetActive(false);
    }
}
