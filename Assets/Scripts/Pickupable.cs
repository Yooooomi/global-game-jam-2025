public interface Pickupable
{
  public bool CanPickup(PlayerPicker from);
  public void Pickup(PlayerPicker picker);
}