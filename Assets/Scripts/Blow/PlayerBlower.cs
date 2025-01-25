using UnityEngine;

public class PlayerBlower : MonoBehaviour
{
    private PlayerStats stats;
    private BlowRangeRegistry registry;
    private PlayerControls controls;
    public float maxForce;
    public float forceDepletionPerMeter;
    public bool blowing;

    public float maxSeconds;
    public float resplenishCooldownAfterUse;
    public float resplenishPerSeconds;
    public float energy { get; private set; }
    private float lastStopped;


    private void Start()
    {
        energy = maxSeconds;
        stats = GetComponent<PlayerStats>();
        registry = GetComponentInChildren<BlowRangeRegistry>();
        controls = GetComponent<PlayerControls>();
    }

    private void MarkStopped()
    {
        if (blowing == false)
        {
            return;
        }
        blowing = false;
        lastStopped = Time.time;
    }

    private void Update()
    {
        if (lastStopped + resplenishCooldownAfterUse > Time.time)
        {
            return;
        }
        if (blowing)
        {
            return;
        }
        energy += resplenishPerSeconds * Time.deltaTime;
        if (energy > maxSeconds)
        {
            energy = maxSeconds;
        }
    }

    private void FixedUpdate()
    {
        if (!stats.alive)
        {
            return;
        }
        if (!controls.blow || energy == 0)
        {
            MarkStopped();
            return;
        }

        energy -= Time.deltaTime;

        if (energy < 0)
        {
            energy = 0;
            MarkStopped();
            return;
        }

        blowing = true;

        foreach (var item in registry.GetInRange())
        {
            if (!item.TryGetComponent<Blowable>(out var blowable))
            {
                continue;
            }
            var distance = Vector3.Distance(transform.position, item.transform.position);
            var force = maxForce - distance * forceDepletionPerMeter;
            if (force > 0)
            {
                blowable.Blow(transform.position, force * Time.deltaTime);
            }
        }
    }
}
