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
    public int minCount = 3;
    public int maxCount = 10;
    public float spawnDelay = 2f;

    private float spawnTimer;

    Bounds bounds;
    public void Start()
    {
        Spawn();
        spawnTimer = spawnDelay;
        bounds = boxCollider.bounds;
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


        int mobCount = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < mobCount; i++)
        {
            Vector3 randomPos = GetRandomPointInBox(boxCollider);
            GameObject temp = Instantiate(Mob, randomPos, SpawnArea.rotation);
            temp.GetComponent<CharacterBrain>().Init(0f, true, false);
        }
    }


    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.center + box.transform.position;
        Vector3 size = box.size;

        float randomX = Random.Range(-size.x / 2f, size.x / 2f);
        float randomY = Random.Range(-size.y / 2f, size.y / 2f);
        float randomZ = Random.Range(-size.z / 2f, size.z / 2f);

        return center + box.transform.rotation * new Vector3(randomX, randomY, randomZ);
    }
}
