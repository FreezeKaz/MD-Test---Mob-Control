using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] public Transform SpawnArea;
    [SerializeField] public BoxCollider boxCollider;
    [SerializeField] public GameObject Mob;

    [Header("Spawn Settings")]
    public int enemyCount = 10;
    public bool useXAxis = true;
    public float spawnDelay = 0.3f;
    public float randomOffset = 0.5f;
    private float spawnTimer;

    Bounds bounds;
    public void Start()
    {
     
        spawnTimer = spawnDelay;
        bounds = boxCollider.bounds;
        Spawn();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            Spawn();
            spawnTimer = spawnDelay; // reset timer
        }
    }
    public void Spawn()
    {



        for (int i = 0; i < enemyCount; i++)
        {
            float t = (float)i / (enemyCount - 1);

            Vector3 spawnPos = Vector3.zero;

            if (useXAxis)
            {
                // Line goes left ↔ right (X axis)
                float x = Mathf.Lerp(bounds.min.x, bounds.max.x, t);
                float z = Random.Range(bounds.min.z, bounds.max.z); // small wiggle in depth
                spawnPos = new Vector3(x, bounds.center.y, z);
            }

            spawnPos += new Vector3(
               Random.Range(-randomOffset, randomOffset),
               0f,
               Random.Range(-randomOffset, randomOffset)
           );
            GameObject temp = Instantiate(Mob, spawnPos, SpawnArea.rotation);
            temp.GetComponent<CharacterBrain>().Init(0f, false, false);
        }



    }
}
