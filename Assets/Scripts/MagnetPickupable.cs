public class MagnetPickupable : Pickupable
{
  public float orbSpeed;

  protected override void Pickup(PlayerPicker picker)
  {
    var orbs = FindObjectsOfType<ExperienceOrb>();
    foreach (var orb in orbs)
    {
      orb.StartPickup(picker, orbSpeed);
    }
  }
}
