using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    public void Explode(float radius, float damage, Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D collider in hits)
        {
            Hittable hittable = collider.GetComponent<Hittable>();
            if (hittable == null)
            {
                continue;
            }
            Debug.Log("hit");
            hittable.Hit(damage);
        }
       GameObject explosion = Instantiate(explosionPrefab,  position, Quaternion.identity);
       explosion.transform.localScale *= radius * 2;
    }
}
