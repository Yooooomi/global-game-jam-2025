using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    public float Explode(float radius, float damage, Vector2 position, bool damagePlayer = false)
    {
        var damages = 0f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D collider in hits)
        {
            Hittable hittable = collider.GetComponent<Hittable>();
            if (hittable == null || (!damagePlayer && collider.CompareTag("Player")))
            {
                continue;
            }
            damages += hittable.Hit(damage);
        }
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.transform.localScale *= radius * 2;
        return damages;
    }
}
