using UnityEngine;

public class PlayerPicker : MonoBehaviour
{
  public float moveSpeed;
  public float pickupRadius;
  public float pickupRange;

  private PlayerUpgrades playerUpgrades;
  private CircleCollider2D collider;

  private void Start()
  {
    playerUpgrades = GetComponentInParent<PlayerUpgrades>();
    collider = GetComponent<CircleCollider2D>();
  }

  private void Update()
  {
    collider.radius = pickupRadius + (playerUpgrades.GetValueByKey("pickup_radius") / 100f * pickupRadius);
  }

  public void OnTriggerStay2D(Collider2D collider)
  {
    if (!collider.TryGetComponent<Pickupable>(out var pickupable))
    {
      return;
    }

    var diff = (collider.transform.position - transform.position).normalized;
    collider.transform.position -= moveSpeed * Time.deltaTime * diff;
    if (Vector3.Distance(transform.position, collider.transform.position) < pickupRange)
    {
      pickupable.Pickup(this);
    }
  }
}
