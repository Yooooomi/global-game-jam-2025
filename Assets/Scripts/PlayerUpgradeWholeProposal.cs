using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeWholeProposal : MonoBehaviour
{
  [SerializeField]
  private GameObject playerUpgradeSpace;
  [SerializeField]
  private RectTransform canvas;
  [SerializeField]
  private List<PlayerUpgradeProposal> proposals = new();

  private HashSet<int> upgraded = new();

  public void Init(List<PlayerUpgrades> upgrades)
  {

    bool atLeastOnePlayerHasToUpgrade = false;

    int i = 0;
    for (i = 0; i < upgrades.Count; i += 1)
    {
      var upgrade = upgrades[i];
      var needsToUpgrade = proposals[i].Init(upgrade, () =>
      {
        upgraded.Add(i);
        if (upgraded.Count == upgrades.Count)
        {
          Time.timeScale = 1f;
          Destroy(gameObject);
        }
      });
      if (needsToUpgrade)
      {
        proposals[i].gameObject.SetActive(true);
        atLeastOnePlayerHasToUpgrade = true;
      }
    }
    for (; i < 4; i += 1)
    {
      proposals[i].gameObject.SetActive(false);
    }
    if (atLeastOnePlayerHasToUpgrade)
    {
      Time.timeScale = 0f;
    }
  }
}