using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float birth;
    private float damage;

    public void Init(Vector3 direction, float speed, float lifetime, float damage)
    {
        birth = Time.time;
        this.direction = direction;
        this.speed = speed;
        this.lifetime = lifetime;
        this.damage = damage;
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
        Destroy(gameObject);
    }
}
