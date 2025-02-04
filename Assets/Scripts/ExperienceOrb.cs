using UnityEngine;

public class ExperienceOrb : Pickupable
{
  public int experience;

  public void Init(int experience)
  {
    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    this.experience = experience;
  }

  public override bool CanPickup(PlayerPicker picker)
  {
    return true;
  }

  protected override void Pickup(PlayerPicker picker)
  {
    if (!GameObject.Find("PlayerManager").TryGetComponent<PlayerExperience>(out var playerExperience))
    {
      return;
    }
    playerExperience.Grant(experience);

    // this is ugly
    GetComponent<RandomSound>().PlayRandom();
  }
}