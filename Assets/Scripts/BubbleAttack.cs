using UnityEngine;

public class BubbleAttack : MonoBehaviour
{
  public float attackSpeed;
  public float damage;
  public float range;
  private float lastAttack;

  private bubble bubble;

  public void Start()
  {
    bubble = GetComponent<bubble>();
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
      if (bubble.target.TryGetComponent<Hittable>(out var hittable))
      {
        hittable.Hit(damage);
        lastAttack = Time.time;
      }
    }
  }
}
