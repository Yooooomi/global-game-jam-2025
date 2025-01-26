using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OnUpgradeEvent : UnityEvent { }

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
  public float alreadyUpgradedAddedChance;
  public List<PlayerUpgrade> availableUpgrades = new();
  public Dictionary<string, int> upgraded = new();
  public OnUpgradeEvent onUpgradeEvent = new();

  public PlayerUpgrade GetByKey(string key)
  {
    var upgrade = availableUpgrades.Find(e => e.key == key);
    return upgrade;
  }

  public bool CanUpgrade()
  {
    foreach (var availableUpgrade in availableUpgrades)
    {
      if (GetLevelByKey(availableUpgrade.key) + 1 != availableUpgrade.values.Length)
      {
        return true;
      }
    }
    return false;
  }

  private float GetValueByKeyAndLevel(string key, int level)
  {
    if (level == -1)
    {
      return 0f;
    }
    var upgrade = GetByKey(key);
    return upgrade.values[level];
  }

  public int GetLevelByKey(string key)
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
    var chances = new List<(int index, float chances)>();

    for (int i = 0; i < availableUpgrades.Count; i += 1)
    {
      var availableUpgrade = availableUpgrades[i];
      var level = GetLevelByKey(availableUpgrade.key);
      var maxedOut = availableUpgrade.values.Length == level + 1;
      if (maxedOut)
      {
        continue;
      }
      var hasSomeLevels = level != -1;
      if (hasSomeLevels)
      {
        chances.Add((i, 1f * alreadyUpgradedAddedChance));
      }
      else
      {
        chances.Add((i, 1f));
      }
    }

    var shuffledChances = chances.OrderBy(_ => Guid.NewGuid()).ToList();
    var pickedUpgrades = new List<int>();

    while (pickedUpgrades.Count != 3 && shuffledChances.Count > 0)
    {
      var totalChances = shuffledChances.Sum(e => e.chances);
      var picked = UnityEngine.Random.Range(0f, totalChances);

      var additiveChance = 0f;
      foreach (var upgradable in shuffledChances)
      {
        additiveChance += upgradable.chances;
        if (picked < additiveChance)
        {
          pickedUpgrades.Add(upgradable.index);
          shuffledChances.Remove(upgradable);
          break;
        }
      }
    }

    return pickedUpgrades.Select(upgradeIndex => availableUpgrades[upgradeIndex]).ToList();
  }

  public void UpgradeByKey(string key)
  {
    if (!upgraded.ContainsKey(key))
    {
      upgraded[key] = 0;
      onUpgradeEvent.Invoke();
      return;
    }
    upgraded[key] += 1;
    onUpgradeEvent.Invoke();
  }
}
