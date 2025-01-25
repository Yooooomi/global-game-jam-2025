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

    private float lastShoot;
    public float baseFirerate;
    public float bulletSpeed;
    public float bulletLifetime;
    public float baseDamage;
    public float spreadAngle;

    public OnKillEvent onKillEvent = new();

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

    private IEnumerator LightningCoroutine(List<Transform> transforms)
    {
        var elec = Instantiate(electric, transforms[0].position - Vector3.forward * 5f, Quaternion.identity);
        var trailRenderer = elec.GetComponent<TrailRenderer>();

        var speed = 45f;
        var randomness = .30f;

        // Start at the first position
        Vector3 currentPosition = transforms[0].position;
        trailRenderer.transform.position = currentPosition + Vector3.forward * -5f;

        for (int i = 1; i < transforms.Count; i++)
        {
            Vector3 nextPosition = transforms[i].position;
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

            // Set the position exactly at the next point
            currentPosition = nextPosition;
            trailRenderer.transform.position = currentPosition + Vector3.forward * -5f;
        }
    }

    private void Electrize(Transform target, HashSet<int> history)
    {
        history.Add(target.GetInstanceID());

        var targets = new List<Transform>() { target };

        // upgrades.GetValueByKey("aoe");

        while (true)
        {
            var colliders = Physics2D.OverlapCircleAll(target.position, 2.5f);
            var first = colliders
                .OrderBy(e => Vector3.Distance(target.position, e.transform.position))
                .FirstOrDefault(e => !history.Contains(e.transform.GetInstanceID()) && e.GetComponent<Hittable>() != null);


            if (first == null)
            {
                break;
            }

            history.Add(first.transform.GetInstanceID());
            target = first.transform;
            targets.Add(target);

            // if (!first.TryGetComponent<Hittable>(out var hittable))
            // {
            //     break;
            // }
        }
        StartCoroutine(LightningCoroutine(targets));
    }

    private void OnHit(int experience, GameObject go)
    {
        if (experience > 0)
        {
            onKillEvent.Invoke(experience);
        }
        // if (upgrades.GetValueByKey("aoe") == -1f)
        // {
        //     return;
        // }
        Electrize(go.transform, new HashSet<int>());
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
