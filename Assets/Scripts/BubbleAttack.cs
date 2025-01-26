using UnityEngine;
using UnityEngine.Events;

public class BubbleAttack : MonoBehaviour
{
  public float attackSpeed;
  public float damage;
  public float range;
  private float lastAttack;

  private BubbleMovement bubble;

  [SerializeField]
  private ExplosiveBubble explosive;

  public void Start()
  {
    bubble = GetComponent<BubbleMovement>();
  }

  public void Update()
  {
    if (bubble.target == null)
    {
      return;
    }
    if (lastAttack + 1f / attackSpeed > Time.time)
    {
      return;
    }
    var targetDistance = Vector3.Distance(transform.position, bubble.target.transform.position);
    if (targetDistance <= range)
    {
      if (explosive.shouldExplode)
      {
        explosive.Explode();
      }
      else if (bubble.target.TryGetComponent<Hittable>(out var hittable))
      {
        hittable.Hit(damage);
        lastAttack = Time.time;
      }
    }
  }
}
