using UnityEngine;

public class BubbleHittable : MonoBehaviour, Hittable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Collider2D selfCollider;
    [SerializeField]
    private RandomSound toPlayOnDeath;

    [SerializeField]
    private float dieAnimationTime;

    public int experience;

    private void Kill()
    {
        selfCollider.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("Dead", true);
        toPlayOnDeath.PlayRandom();
        Destroy(gameObject, dieAnimationTime);
    }

    public int Hit(float damage)
    {
        Kill();
        return experience;
    }
}
