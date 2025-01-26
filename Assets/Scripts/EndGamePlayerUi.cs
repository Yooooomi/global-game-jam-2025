using TMPro;
using UnityEngine;

public class EndGamePlayerUi : MonoBehaviour
{
  public TMP_Text total;
  public TMP_Text weapon;
  public TMP_Text explosion;
  public TMP_Text boomerang;
  public TMP_Text saw;
  public TMP_Text electrify;
  public TMP_Text taken;

  public void Init(PlayerGameStats stats)
  {
    total.text = stats.damages.ToString();
    weapon.text = stats.GetDamagesOfWeapon(WeaponType.weapon).ToString();
    explosion.text = stats.GetDamagesOfWeapon(WeaponType.explosion).ToString();
    boomerang.text = stats.GetDamagesOfWeapon(WeaponType.boomerang).ToString();
    saw.text = stats.GetDamagesOfWeapon(WeaponType.rotatingSaw).ToString();
    electrify.text = stats.GetDamagesOfWeapon(WeaponType.electrify).ToString();
    taken.text = stats.damageTaken.ToString();
  }
}