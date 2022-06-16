using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    [SerializeField] bool hasHazard = false;
    public float speed = 3f;

    public bool HasHazard => hasHazard;


    void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ChunkKiller"))
            gameObject.SetActive(false);
    }
}
