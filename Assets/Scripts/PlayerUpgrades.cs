using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerUpgrade
{
  public string key;
  public string name;
  public float[] values;
  // You can use {{diffValue}} and {{value}} to insert value in string
  public string description;
}

public class PlayerUpgrades : MonoBehaviour
{
  public List<PlayerUpgrade> availableUpgrades = new();
  public Dictionary<string, int> upgraded = new();

  public PlayerUpgrade GetByKey(string key)
  {
    var upgrade = availableUpgrades.Find(e => e.key == key);
    return upgrade;
  }

  private float GetValueByKeyAndLevel(string key, int level)
  {
    if (level == -1) {
      return 0f;
    }
    var upgrade = GetByKey(key);
    return upgrade.values[level];
  }

  private int GetLevelByKey(string key)
  {
    if (!upgraded.TryGetValue(key, out int level))
    {
      return -1;
    }
    return level;
  }

  public float GetValueByKey(string key)
  {
    return GetValueByKeyAndLevel(key, GetLevelByKey(key));
  }

  public string GetDescriptionByKey(string key)
  {
    var upgrade = GetByKey(key);

    var description = upgrade.description;

    var thisLevel = GetLevelByKey(key);

    var nextLevelValue = GetValueByKeyAndLevel(key, thisLevel + 1);
    var diff = nextLevelValue - GetValueByKeyAndLevel(key, thisLevel);

    var interpolatedDescription = description.Replace("{{diffValue}}", diff.ToString()).Replace("{{value}}", nextLevelValue.ToString());
    return interpolatedDescription;
  }

  public List<PlayerUpgrade> GetNextUpgrades()
  {

    var random = new System.Random();
    var selectedIndices = new HashSet<int>();

    var playerAvailableUpgrades = availableUpgrades
      .Where(e => !upgraded.ContainsKey(e.key) || upgraded[e.key] < availableUpgrades.Find(upgrade => upgrade.key == e.key).values.Length - 1)
      .OrderBy(_ => Guid.NewGuid())
      .Take(3)
      .ToList();

    return playerAvailableUpgrades;
  }

  public void UpgradeByKey(string key)
  {
    if (!upgraded.ContainsKey(key))
    {
      upgraded[key] = 0;
      return;
    }
    upgraded[key] += 1;
  }
}
