using UnityEngine;

public class RotatingSaw : MonoBehaviour
{
  private RotatingSawWeapon rotatingSawWeapon;
  private PlayerWeapon weapon;
  private RandomSound sound;
  private PlayerGameStats playerGameStats;

  private void Start()
  {
    rotatingSawWeapon = GetComponentInParent<RotatingSawWeapon>();
    weapon = GetComponentInParent<PlayerWeapon>();
    sound = GetComponent<RandomSound>();
    playerGameStats = GetComponentInParent<PlayerGameStats>();
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.CompareTag("Player") || !collider.TryGetComponent<Hittable>(out var hittable))
    {
      return;
    }
    playerGameStats.RegisterDamages(WeaponType.rotatingSaw, hittable.Hit(Mathf.Ceil(weapon.GetDamage() * rotatingSawWeapon.weaponDamageMultiplier)));
    sound.PlayRandom(.5f);
  }
}