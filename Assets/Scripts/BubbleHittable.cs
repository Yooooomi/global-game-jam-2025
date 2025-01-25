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
    public int experience;

    private void Kill()
    {
        selfCollider.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        toPlayOnDeath.PlayRandom();
        Destroy(gameObject, dieDelaySec);
    }

    public int Hit(float damage)
    {
        hp -= damage;
        OnHealthChanged.Invoke(hp);
        if (hp <= 0) {
            Kill();
            return experience;
        }
        return -1;
    }
}
