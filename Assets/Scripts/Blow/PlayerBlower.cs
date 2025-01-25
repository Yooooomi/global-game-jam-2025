using UnityEngine;

public class PlayerBlower : MonoBehaviour
{
    private BlowRangeRegistry registry;
    private PlayerControls controls;
    public float maxForce;
    public float forceDepletionPerMeter;
    [SerializeField]
    private ParticleSystem particleSystem;

    private void Start()
    {
        registry = GetComponentInChildren<BlowRangeRegistry>();
        controls = GetComponent<PlayerControls>();
    }

    private void FixedUpdate()
    {
        if (!controls.blow)
        {
            particleSystem.Stop();
            return;
        }
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
            var main = particleSystem.main;
            main.startSpeed = 20f;
        }

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
