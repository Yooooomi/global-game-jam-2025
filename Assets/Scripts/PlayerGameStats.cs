using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
  weapon = 0,
  explosion = 1,
  boomerang = 2,
  rotatingSaw = 3,
  electrify = 4,
}

public class PlayerGameStats : MonoBehaviour
{
  public float damages;
  public float damageTaken;
  private readonly Dictionary<WeaponType, float> weaponDamages = new();

  private void Start()
  {
    weaponDamages[WeaponType.weapon] = 0;
    weaponDamages[WeaponType.explosion] = 0;
    weaponDamages[WeaponType.boomerang] = 0;
    weaponDamages[WeaponType.rotatingSaw] = 0;
    weaponDamages[WeaponType.electrify] = 0;
  }

  public void RegisterDamages(WeaponType weaponType, float damages)
  {
    damages += damages;
    weaponDamages[weaponType] += damages;
  }

  public void TakeDamages(float damageTaken)
  {
    this.damageTaken += damageTaken;
  }

  public float GetDamagesOfWeapon(WeaponType weaponType)
  {
    return weaponDamages[weaponType];
  }
}