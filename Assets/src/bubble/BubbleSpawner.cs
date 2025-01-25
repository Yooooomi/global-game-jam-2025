using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;

    // Specs of the bubbles
    [System.Serializable]
    private struct BubbleSpec
    {
        public int hp;
        public BubbleKind kind;
    }
    [System.Serializable]
    private enum BubbleKind
    {
        Blue,
        Purple,
    };
    [SerializeField]
    private List<BubbleSpec> bubble_specs;
    private Dictionary<BubbleKind, BubbleSpec> bubble_spec_dic = new();


    // Steps in the game
    [System.Serializable]
    private struct BubbleSpawnSpec
    {
        public BubbleKind kind;
        public int numberPerBatch;
    }
    [System.Serializable]
    private struct SpawnStep
    {
        public List<BubbleSpawnSpec> bubbles;
    }
    [SerializeField]
    private List<SpawnStep> steps;

    // Spawn cadence
    [SerializeField]
    private float spawnEvery;
    [SerializeField]
    private float nextStepEvery;

    // Spawn position
    [SerializeField]
    private float minRadius = 5.0f;
    [SerializeField]
    private float maxRadius = 6.0f;

    // Internal
    private float lastSpawnTime = 0;
    private int stepIndex = 0;

    void SpawnBubble(BubbleSpec spec)
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized;

        float randomRadius = Random.Range(minRadius, maxRadius);
        randomPos *= randomRadius;

        Vector2 spawnPos = (Vector2)transform.position + randomPos;

        GameObject newBubble = Instantiate(bubble, spawnPos, Quaternion.identity);
        newBubble.GetComponent<BubbleHittable>().SetInitialHp(spec.hp);
    }

    void SpawnBubbles()
    {
        if (stepIndex >= steps.Count)
        {
            stepIndex = steps.Count - 1;
        }
        SpawnStep step = steps[stepIndex];
        foreach (BubbleSpawnSpec spawn_spec in step.bubbles)
        {
            for (int i = 0; i < spawn_spec.numberPerBatch; ++i)
            {
                SpawnBubble(bubble_spec_dic[spawn_spec.kind]);
            }
        }
    }

    void Start()
    {
        foreach (BubbleSpec spec in bubble_specs)
        {
            bubble_spec_dic.Add(spec.kind, spec);
        }
    }

    void Update()
    {
        stepIndex = Mathf.FloorToInt(Time.time / nextStepEvery);
    }

    void FixedUpdate()
    {
        if (Time.time > lastSpawnTime + spawnEvery)
        {
            SpawnBubbles();
            lastSpawnTime = Time.time;
        }
    }
}
