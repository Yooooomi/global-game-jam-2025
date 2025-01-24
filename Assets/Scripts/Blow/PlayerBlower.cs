using UnityEngine;

public class PlayerBlower : MonoBehaviour
{
    private BlowRangeRegistry registry;
    private PlayerControls controls;
    public float maxForce;
    public float forceDepletionPerMeter;

    private void Start()
    {
        registry = GetComponentInChildren<BlowRangeRegistry>();
        controls = GetComponent<PlayerControls>();
    }

    private void FixedUpdate()
    {
        if (!controls.blow)
        {
            return;
        }
        foreach (var item in registry.inRange)
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
