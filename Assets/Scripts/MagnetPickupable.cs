using UnityEngine;

public class MagnetPickupable : MonoBehaviour, Pickupable
{
  public bool CanPickup(PlayerPicker from)
  {
    return true;
  }

  public void Pickup(PlayerPicker picker)
  {
    var orbs = FindObjectsOfType<ExperienceOrb>();
    foreach (var orb in orbs)
    {
      orb.GoTo(picker);
    }
    Destroy(gameObject);
  }
}
