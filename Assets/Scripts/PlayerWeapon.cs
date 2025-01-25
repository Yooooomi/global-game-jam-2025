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

    private float lastShoot;
    public float baseFirerate;
    public float bulletSpeed;
    public float bulletLifetime;
    public float damage;

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

    private void OnKill(int experience)
    {
        onKillEvent.Invoke(experience);
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

        var b = Instantiate(bullet, canon.transform.position, Quaternion.identity);
        if (!b.TryGetComponent<Bullet>(out var bull))
        {
            Destroy(b);
            return;
        }
        bull.Init(canon.transform.up, bulletSpeed, bulletLifetime, damage, OnKill);
    }
}
