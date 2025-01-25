using UnityEngine;

public class ExperienceOrb : MonoBehaviour, Pickupable
{
  public int experience;
  public float goingSpeed;
  private PlayerPicker going;

  public void Init(int experience)
  {
    transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    this.experience = experience;
  }

  public bool CanPickup(PlayerPicker from)
  {
    return going == null || going == from.gameObject;
  }

  public void GoTo(PlayerPicker going)
  {
    this.going = going;
  }

  private void Update()
  {
    if (going == null)
    {
      return;
    }
    var direction = (going.transform.position - transform.position).normalized;
    transform.position += goingSpeed * Time.deltaTime * direction;
  }

  public void Pickup(PlayerPicker picker)
  {
    if (!GameObject.Find("PlayerManager").TryGetComponent<PlayerExperience>(out var playerExperience))
    {
      return;
    }
    playerExperience.Grant(experience);

    // this is ugly
    GetComponent<RandomSound>().PlayRandom();

    Destroy(gameObject);
  }
}