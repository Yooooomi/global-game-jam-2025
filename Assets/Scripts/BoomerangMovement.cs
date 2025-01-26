using System;
using System.Collections;
using UnityEngine;

public class BoomerangMovement : Pickupable
{
  public float time;
  public float speed;
  public float returnTime;
  public float returnAngle;

  private PlayerWeapon playerWeapon;
  private Action<GameObject> onHitPlayer;
  private float weaponDamageMultiplier;
  private bool onReturn;
  private RandomSound sound;
  private Coroutine currentMovement;
  private PlayerGameStats playerGameStats;

  private void Start()
  {
    sound = GetComponent<RandomSound>();
    currentMovement = StartCoroutine(PerformMovement());
  }

  public void Init(float weaponDamageMultiplier, PlayerWeapon playerWeapon, PlayerGameStats playerGameStats, Action<GameObject> onHitPlayer)
  {
    this.playerWeapon = playerWeapon;
    this.onHitPlayer = onHitPlayer;
    this.weaponDamageMultiplier = weaponDamageMultiplier;
    this.playerGameStats = playerGameStats;
  }

  private IEnumerator PerformMovement()
  {
    var angle = UnityEngine.Random.Range(0f, 360f);

    transform.rotation = Quaternion.Euler(0f, 0f, angle);

    var elapsed = 0f;

    while (elapsed < time)
    {
      transform.position += speed * Time.deltaTime * transform.up;

      elapsed += Time.deltaTime;
      yield return null;
    }

    angle = UnityEngine.Random.Range(180f - returnAngle, 180f + returnAngle);
    transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + angle);

    elapsed = 0f;

    onReturn = true;

    while (elapsed < returnTime)
    {
      transform.position += speed * Time.deltaTime * transform.up;

      elapsed += Time.deltaTime;
      yield return null;
    }
    Destroy(gameObject);
  }

  public override bool CanPickup(PlayerPicker picker)
  {
    return onReturn;
  }

  protected override void Pickup(PlayerPicker picker)
  {
    onHitPlayer(picker.gameObject);
  }

  protected override void OnStartPickup()
  {
    base.OnStartPickup();
    if (currentMovement == null)
    {
      return;
    }
    StopCoroutine(currentMovement);
    currentMovement = null;
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (!onReturn)
    {
      return;
    }
    if (collider.gameObject.CompareTag("Player"))
    {
      return;
    }
    if (!collider.TryGetComponent<Hittable>(out var hittable))
    {
      return;
    }
    playerGameStats.RegisterDamages(WeaponType.boomerang, hittable.Hit(Mathf.Ceil(playerWeapon.GetDamage() * weaponDamageMultiplier)));
    sound.PlayRandom(.5f);
  }
}
