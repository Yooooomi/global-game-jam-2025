using UnityEngine;
using UnityEngine.Events;

public class BubbleHittable : MonoBehaviour, Hittable
{

    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Collider2D selfCollider;
    [SerializeField]
    private RandomSound toPlayOnDeath;

    [SerializeField]
    private float dieDelaySec;

    public UnityEvent<float> OnHealthChanged;
    public float hp;

    private void Kill() {
        selfCollider.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        toPlayOnDeath.PlayRandom();
        Destroy(gameObject, dieDelaySec);
    }

    public void Hit(float damage)
    {
        hp -= damage;
        if (hp <= 0) {
            Kill();
        }
        OnHealthChanged.Invoke(hp);
    }
}
