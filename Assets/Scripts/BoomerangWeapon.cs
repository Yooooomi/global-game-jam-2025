using UnityEngine;

public class BoomerangWeapon : MonoBehaviour
{
  public float weaponDamageMultiplier;
  private PlayerUpgrades upgrades;

  private float lastTimeLaunched;
  [SerializeField]
  private GameObject boomerangPrefab;
  private PlayerWeapon weapon;

  private void Start()
  {
    upgrades = GetComponentInParent<PlayerUpgrades>();
    weapon = GetComponentInParent<PlayerWeapon>();
  }

  private void Launch()
  {
    var go = Instantiate(boomerangPrefab, transform.position, Quaternion.identity, null);
    var boomerang = go.GetComponent<BoomerangMovement>();
    boomerang.Init(weaponDamageMultiplier, weapon, OnHitPlayer);
  }

  private void OnHitPlayer(GameObject player)
  {
    var boomerang = player.GetComponentInChildren<BoomerangWeapon>();
    if (boomerang == null)
    {
      return;
    }
    boomerang.Launch();
  }

  private void Update()
  {
    var value = upgrades.GetValueByKey("boomerang");

    if (value == 0f)
    {
      return;
    }

    if (lastTimeLaunched + value > Time.time)
    {
      return;
    }

    lastTimeLaunched = Time.time;
    Launch();
  }
}