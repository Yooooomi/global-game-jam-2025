using System;
using System.Collections;
using UnityEngine;

public class BoomerangMovement : MonoBehaviour
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

  private void Start()
  {
    sound = GetComponent<RandomSound>();
    StartCoroutine(PerformMovement());
  }

  public void Init(float weaponDamageMultiplier, PlayerWeapon playerWeapon, Action<GameObject> onHitPlayer)
  {
    this.playerWeapon = playerWeapon;
    this.onHitPlayer = onHitPlayer;
    this.weaponDamageMultiplier = weaponDamageMultiplier;
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

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.CompareTag("Player"))
    {
      if (!onReturn)
      {
        return;
      }
      onHitPlayer(collider.gameObject);
      Destroy(gameObject);
      return;
    }
    if (!collider.TryGetComponent<Hittable>(out var hittable))
    {
      return;
    }
    hittable.Hit(Mathf.Ceil(playerWeapon.GetDamage() * weaponDamageMultiplier));
    sound.PlayRandom(.5f);
  }
}
