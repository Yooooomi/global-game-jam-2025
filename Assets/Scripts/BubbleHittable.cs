using System.Xml.XPath;
using UnityEngine;
using UnityEngine.Events;

public class OnBubbleHealthChanged : UnityEvent<float> { }
public class OnBubbleDeath : UnityEvent { }

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

    public OnBubbleHealthChanged onHealthChanged = new();
    public OnBubbleDeath onBubbleDeath = new();
    public float hp;
    public int experience = 0;

    private void Kill()
    {
        selfCollider.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        toPlayOnDeath.PlayRandom();
        Destroy(gameObject, dieDelaySec);
        onBubbleDeath.Invoke();
    }

    public void SetInitialHp(float initialHp)
    {
        hp = initialHp;
        onHealthChanged.Invoke(hp);
    }

    public void SetInitialXp(int xp)
    {
        experience = xp;
    }

    public bool CanHit()
    {
        return MapConfiguration.instance.InMap(transform.position);
    }

    public float Hit(float damage)
    {
        hp -= damage;
        onHealthChanged.Invoke(hp);
        if (hp <= 0)
        {
            Kill();
            return damage + hp;
        }
        return damage;
    }
}
