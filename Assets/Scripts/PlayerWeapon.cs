using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform canon;

    private float lastShoot;
    public float firerate;
    public float bulletSpeed;
    public float bulletLifetime;
    public float damage;

    private void Start()
    {
        controls = GetComponent<PlayerControls>();
    }

    private void Update()
    {
        if (Time.time < lastShoot + (1f / firerate))
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
        bull.Init(canon.transform.up, bulletSpeed, bulletLifetime, damage);
    }
}
