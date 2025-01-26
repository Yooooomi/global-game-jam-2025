using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float birth;
    private float damage;

    private Collider2D myCollider;
    private SpriteRenderer sprite;
    private ParticleSystem particleSystem;
    private Action<GameObject> onHit;
    private PlayerGameStats playerGameStats;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void Init(Vector3 direction, float speed, float lifetime, float damage, Action<GameObject> onHit, PlayerGameStats playerGameStats)
    {
        birth = Time.time;
        this.direction = direction;
        this.speed = speed;
        this.lifetime = lifetime;
        this.damage = damage;
        this.onHit = onHit;
        this.playerGameStats = playerGameStats;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        if (birth + lifetime < Time.time)
        {
            Destroy(gameObject);
            return;
        }
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent<Hittable>(out var hittable))
        {
            return;
        }
        if (collider.gameObject.CompareTag("Player"))
        {
            return;
        }
        playerGameStats.RegisterDamages(WeaponType.weapon, hittable.Hit(damage));
        onHit(collider.gameObject);
        Destroy(gameObject, 5f);
        sprite.enabled = false;
        myCollider.enabled = false;
        particleSystem.Stop();
    }
}
