using System;
using System.Collections.Generic;
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
    var upgrade = GetByKey(key);
    if (upgrade == null)
    {
      return 0f;
    }
    return upgrade.values[level];
  }

  private int GetLevelByKey(string key)
  {
    if (!upgraded.TryGetValue(key, out int level))
    {
      return 0;
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

    System.Random random = new System.Random();
    HashSet<int> selectedIndices = new HashSet<int>();

    while (selectedIndices.Count < 3)
    {
      int randomIndex = random.Next(availableUpgrades.Count);
      selectedIndices.Add(randomIndex);
    }

    List<PlayerUpgrade> randomElements = new();
    foreach (int index in selectedIndices)
    {
      randomElements.Add(availableUpgrades[index]);
    }

    return randomElements;
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
