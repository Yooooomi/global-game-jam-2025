using UnityEngine;

public class CanonRotation : MonoBehaviour
{
  public float startRadiusCheck;
  public float incrementRadiusCheck;
  private PlayerControls controls;
  private PlayerUpgrades upgrades;

  private void Start()
  {
    controls = GetComponentInParent<PlayerControls>();
    upgrades = GetComponentInParent<PlayerUpgrades>();
  }

  public void UpdateAim()
  {
    if (upgrades.GetValueByKey("auto_aim") == 0f)
    {
      return;
    }
    BubbleHittable leastHittable = null;
    var radiusCheck = startRadiusCheck;
    while (radiusCheck < 100f && leastHittable == null)
    {
      var colliders = Physics2D.OverlapCircleAll(transform.position, radiusCheck);
      radiusCheck += incrementRadiusCheck;
      if (colliders.Length == 0)
      {
        continue;
      }
      var leastValue = -1f;
      for (int i = 0; i < colliders.Length; i += 1)
      {
        if (!colliders[i].TryGetComponent<BubbleHittable>(out var bubbleHittable))
        {
          continue;
        }
        var value = (colliders[i].transform.position - transform.position).sqrMagnitude;
        if (leastValue == -1f || value < leastValue)
        {
          leastValue = value;
          leastHittable = bubbleHittable;
        }
      }
    }
    if (leastHittable != null)
    {
      var direction = leastHittable.transform.position - transform.position;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
      transform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }

  private void Update()
  {
    if (upgrades.GetValueByKey("auto_aim") == 0f)
    {
      float angle = Mathf.Atan2(controls.look.y, controls.look.x) * Mathf.Rad2Deg - 90f;
      transform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }
}