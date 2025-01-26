using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;

    [System.Serializable]
    private struct BubbleColorOverride
    {
        public bool enabled;
        public BubbleColorPalette palette;
    }

    // Specs of the bubbles
    [System.Serializable]
    private struct BubbleSpec
    {
        public int hp;
        public int xp;
        public BubbleKind kind;
        public float speed;
        public float size;
        public BubbleColorOverride colorOverride;
        public AnimatorOverrideController animOverride;
    }
    [System.Serializable]
    private enum BubbleKind
    {
        Blue,
        Purple,
        Orange,
        Red,
        Green,
        Explosive,
        PinkBoss,
        FastBlue,
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
        public int maxPerStep;
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
    private Dictionary<int, Dictionary<BubbleKind, int>> total_bubble_spawned_by_step;

    void SpawnBubble(BubbleSpec spec)
    {
        Vector2 randomPos = Random.insideUnitCircle.normalized;

        float randomRadius = Random.Range(minRadius, maxRadius);
        randomPos *= randomRadius;

        Vector2 spawnPos = (Vector2)transform.position + randomPos;

        GameObject newBubble = Instantiate(bubble, spawnPos, Quaternion.identity);
        BubbleHittable bubble_stats = newBubble.GetComponent<BubbleHittable>();
        bubble_stats.SetInitialHp(spec.hp);
        bubble_stats.SetInitialXp(spec.xp);
        if (spec.colorOverride.enabled)
        {
            BubbleColor bubble_color = newBubble.GetComponentInChildren<BubbleColor>();
            bubble_color.OverrideBubbleColors(spec.colorOverride.palette);
        }
        BubbleMovement movement = newBubble.GetComponent<BubbleMovement>();
        movement.SetSpeed(spec.speed);
        newBubble.transform.localScale *= spec.size;
        if (spec.animOverride != null)
        {
            var animator = newBubble.GetComponentInChildren<Animator>();
            animator.runtimeAnimatorController = spec.animOverride;
        }
        if (spec.kind == BubbleKind.Explosive)
        {
            newBubble.GetComponent<ExplosiveBubble>().shouldExplode = true;
        }
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
                if (spawn_spec.maxPerStep != 0 && total_bubble_spawned_by_step[stepIndex][spawn_spec.kind] >= spawn_spec.maxPerStep)
                {
                    continue;
                }
                SpawnBubble(bubble_spec_dic[spawn_spec.kind]);
                total_bubble_spawned_by_step[stepIndex][spawn_spec.kind]++;
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
