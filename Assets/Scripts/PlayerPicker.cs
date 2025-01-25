using UnityEngine;

public class PlayerPicker : MonoBehaviour
{
  public float pickupSpeed;
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

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (!collider.TryGetComponent<Pickupable>(out var pickupable))
    {
      return;
    }
    pickupable.StartPickup(this, pickupSpeed);
  }
}
