using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;

    // Spawn cadence
    [SerializeField]
    private float spawnEverySec;
    [SerializeField]
    private float bubbleToSpawnPerBatch;
    private float lastSpawnTime;

    // Span position
    [SerializeField]
    private float minRadius = 5.0f;
    [SerializeField]
    private float maxRadius = 6.0f;

    void SpawnBubble() {
        Vector2 randomPos = Random.insideUnitCircle.normalized;

        float randomRadius = Random.Range(minRadius, maxRadius);
        randomPos *= randomRadius;

        Vector2 spawnPos = (Vector2)transform.position + randomPos;

        Instantiate(bubble, spawnPos, Quaternion.identity);
    }

    void SpawnBubbles() {
        for (int i = 0; i < bubbleToSpawnPerBatch; ++i) {
            SpawnBubble();
        }
    }

    void Start() {
        SpawnBubbles();
        lastSpawnTime = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time > lastSpawnTime + spawnEverySec) {
            SpawnBubbles();
            lastSpawnTime = Time.time;
        }
    }
}
