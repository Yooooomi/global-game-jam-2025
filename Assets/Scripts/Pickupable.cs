using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
  protected abstract void Pickup(PlayerPicker picker);

  private PlayerPicker pickuper;
  private float speed;
  private float pickupAt;
  protected bool pickingUp;

  public void StartPickup(PlayerPicker picker, float speed)
  {
    pickuper = picker;
    pickupAt = Time.time + Vector3.Distance(transform.position, picker.transform.position) / speed;
    this.speed = speed;
  }

  protected virtual void OnStartPickup() { }

  protected virtual void Update()
  {
    if (pickuper == null)
    {
      return;
    }
    var direction = pickuper.transform.position - transform.position;
    transform.position += Time.deltaTime * speed * direction;
    if (Time.time > pickupAt)
    {
      Pickup(pickuper);
      Destroy(gameObject);
    }
  }
}