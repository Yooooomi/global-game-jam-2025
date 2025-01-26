using UnityEngine;

public class RotatingSaw : MonoBehaviour
{
  private RotatingSawWeapon rotatingSawWeapon;
  private PlayerWeapon weapon;
  private RandomSound sound;

  private void Start()
  {
    rotatingSawWeapon = GetComponentInParent<RotatingSawWeapon>();
    weapon = GetComponentInParent<PlayerWeapon>();
    sound = GetComponent<RandomSound>();
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Player") || !collider.TryGetComponent<Hittable>(out var hittable))
    {
      return;
    }
    hittable.Hit(Mathf.Ceil(weapon.GetDamage() * rotatingSawWeapon.weaponDamageMultiplier));
    sound.PlayRandom(.5f);
  }
}