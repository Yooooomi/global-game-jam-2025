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

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void Init(Vector3 direction, float speed, float lifetime, float damage)
    {
        birth = Time.time;
        this.direction = direction;
        this.speed = speed;
        this.lifetime = lifetime;
        this.damage = damage;

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
        hittable.Hit(damage);
        Destroy(gameObject, 5f);
        sprite.enabled = false;
        myCollider.enabled = false;
        particleSystem.Stop();
    }
}
