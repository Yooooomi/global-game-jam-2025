using System.Collections.Generic;
using UnityEngine;

public class RotatingSawWeapon : MonoBehaviour
{
  public float radius;
  public float baseSpeed;
  public float speedMultiplierPerSaw;
  public float weaponDamageMultiplier;
  [SerializeField]
  private GameObject rotatingParent;
  [SerializeField]
  private GameObject sawPrefab;
  private readonly List<GameObject> saws = new();
  private PlayerUpgrades upgrades;

  private void Start()
  {
    upgrades = GetComponentInParent<PlayerUpgrades>();
    upgrades.onUpgradeEvent.AddListener(OnUpgrade);
  }

  private void OnUpgrade()
  {
    var count = Mathf.FloorToInt(upgrades.GetValueByKey("rotating_saw"));

    if (count == saws.Count)
    {
      return;
    }

    while (saws.Count < count)
    {
      AddSaw();
    }
    UpdateSawPositions();
  }

  private void UpdateSawPositions()
  {
    var count = saws.Count;
    var angleStep = 360f / count;

    for (int i = 0; i < count; i++)
    {
      var angleInDegrees = i * angleStep;
      var angleInRadians = angleInDegrees * Mathf.Deg2Rad;

      var x = radius * Mathf.Cos(angleInRadians);
      var y = radius * Mathf.Sin(angleInRadians);
      var newPosition = new Vector3(x, y, 0f);

      saws[i].transform.localPosition = newPosition;
    }
  }

  private void AddSaw()
  {
    var go = Instantiate(sawPrefab, Vector3.zero, Quaternion.identity, rotatingParent.transform);
    saws.Add(go);
  }

  private void Update()
  {
    var angle = rotatingParent.transform.localRotation.eulerAngles.z;
    angle += baseSpeed * (1f + (saws.Count * (speedMultiplierPerSaw - 1f))) * Time.deltaTime;
    rotatingParent.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
  }
}
