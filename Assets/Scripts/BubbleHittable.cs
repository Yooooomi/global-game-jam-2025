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

    private float killAtTime = 0f;

    private void Kill() {
        selfCollider.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        killAtTime = Time.time + dieAnimationTime;
        animator.SetBool("Dead", true);
        toPlayOnDeath.PlayRandom();
    }

    public void Hit(float damage)
    {
        Kill();
    }

    public void Update() {
        if (killAtTime != 0 && Time.time > killAtTime) {
            Destroy(gameObject);
        }
    }
}
