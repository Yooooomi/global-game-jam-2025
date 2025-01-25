using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnKillEvent : UnityEvent<int> { }

public class PlayerWeapon : MonoBehaviour
{
    private PlayerUpgrades upgrades;
    private PlayerStats stats;
    private PlayerControls controls;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform canon;
    [SerializeField]
    private GameObject electric;
    [SerializeField]
    private Explosion gunExplosion;
    [SerializeField]
    private float electricityDamageMultiplier = 0.5f;

    private float lastShoot;
    public float baseFirerate;
    public float bulletSpeed;
    public float bulletLifetime;
    public float baseDamage;
    public float spreadAngle;

    public OnKillEvent onKillEvent = new();

    [SerializeField]
    private RandomSound electrify;
    [SerializeField]
    private RandomSound firing;

    private void Start()
    {
        upgrades = GetComponent<PlayerUpgrades>();
        stats = GetComponent<PlayerStats>();
        controls = GetComponent<PlayerControls>();
    }

    private float GetFirerate()
    {
        return baseFirerate + upgrades.GetValueByKey("firerate");
    }

    private float GetDamage()
    {
        return baseDamage + upgrades.GetValueByKey("damage");
    }

    private int GetProjectileCount()
    {
        return 1 + Mathf.FloorToInt(upgrades.GetValueByKey("projectile_count"));
    }

    private IEnumerator LightningCoroutine(Transform target)
    {
        var elec = Instantiate(electric, target.position - Vector3.forward * 5f, Quaternion.identity);
        var trailRenderer = elec.GetComponent<TrailRenderer>();

        var speed = 45f;
        var randomness = .30f;

        // Start at the first position
        Vector3 currentPosition = target.position;
        trailRenderer.transform.position = currentPosition + Vector3.forward * -5f;

        var probability = upgrades.GetValueByKey("electrify") / 100f;

        var history = new HashSet<int>
        {
            target.GetInstanceID()
        };

        while (true)
        {
            var colliders = Physics2D.OverlapCircleAll(target.position, 2.5f);
            var first = colliders
                .OrderBy(e => Vector3.Distance(target.position, e.transform.position))
                .FirstOrDefault(e => !e.gameObject.CompareTag("Player") && !history.Contains(e.transform.GetInstanceID()) && e.GetComponent<Hittable>() != null);

            if (first == null)
            {
                break;
            }

            history.Add(first.transform.GetInstanceID());
            target = first.transform;

            Vector3 nextPosition = target.position;
            float distance = Vector3.Distance(currentPosition, nextPosition);
            float timeToMove = distance / speed;

            // Interpolate positions with jagged randomness
            float elapsedTime = 0f;
            while (elapsedTime < timeToMove)
            {
                elapsedTime += Time.deltaTime;

                // Lerp to next position
                float t = Mathf.Clamp01(elapsedTime / timeToMove);
                Vector3 interpolatedPosition = Vector3.Lerp(currentPosition, nextPosition, t);

                // Add random offset for jagged lightning effect
                Vector3 randomOffset = new Vector3(
                    Random.Range(-randomness, randomness),
                    Random.Range(-randomness, randomness),
                    Random.Range(-randomness, randomness)
                );

                trailRenderer.transform.position = interpolatedPosition + randomOffset + Vector3.forward * -5f;

                yield return null;
            }

            if (target == null)
            {
                break;
            }

            if (target.TryGetComponent<Hittable>(out var hittable))
            {
                hittable.Hit(Mathf.CeilToInt(GetDamage() * electricityDamageMultiplier));
                electrify.PlayRandom(.3f);
            }
            // Set the position exactly at the next point
            currentPosition = nextPosition;
            trailRenderer.transform.position = currentPosition + Vector3.forward * -5f;

            if (Random.Range(0f, 1f) > probability)
            {
                break;
            }
        }
    }

    private void Explose(float radius, float damage, Vector2 position)
    {
        gunExplosion.Explode(radius, damage, position);
    }

    private void OnHit(int experience, GameObject go)
    {
        if (experience > 0)
        {
            onKillEvent.Invoke(experience);
        }
        if (upgrades.GetValueByKey("electrify") != 0f)
        {
            StartCoroutine(LightningCoroutine(go.transform));
        }
        float blastRadius = upgrades.GetValueByKey("explosion_radius");
        if (blastRadius != 0f)
        {
            Explose(/*radius=*/blastRadius, /*damage=*/1, go.transform.position);
        }
    }

    private void Update()
    {
        if (!stats.alive)
        {
            return;
        }
        if (Time.time < lastShoot + (1f / GetFirerate()))
        {
            return;
        }
        if (!controls.shoot)
        {
            return;
        }
        lastShoot = Time.time;

        firing.PlayRandom();

        var halfSpread = spreadAngle / 2f;
        var projectileCount = GetProjectileCount();

        for (int i = 0; i < projectileCount; i++)
        {
            float angleStep = projectileCount == 1 ? 0 : spreadAngle / (projectileCount - 1);
            float angle = projectileCount == 1 ? 0 : -halfSpread + (angleStep * i);

            GameObject b = Instantiate(bullet, canon.transform.position, Quaternion.identity);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * canon.transform.up;

            if (!b.TryGetComponent<Bullet>(out var bull))
            {
                Destroy(b);
                return;
            }
            bull.Init(direction, bulletSpeed, bulletLifetime, GetDamage(), OnHit);
        }
    }
}
