using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeWholeProposal : MonoBehaviour
{
  [SerializeField]
  private GameObject playerUpgradeSpace;
  [SerializeField]
  private RectTransform canvas;

  private HashSet<int> upgraded = new();

  public void Init(List<PlayerUpgrades> upgrades)
  {
    Time.timeScale = 0f;
    for (int i = 0; i < upgrades.Count; i += 1)
    {
      var upgrade = upgrades[i];
      var go = Instantiate(playerUpgradeSpace, Vector3.zero, Quaternion.identity, canvas.transform);
      if (!go.TryGetComponent<PlayerUpgradeProposal>(out var proposal))
      {
        return;
      }
      if (!go.TryGetComponent<RectTransform>(out var rectTransform))
      {
        return;
      }
      rectTransform.localScale = Vector3.one * .5f;
      if (i == 0)
      {
        rectTransform.localPosition = new Vector2(-canvas.rect.width / 4f, canvas.rect.height / 4f);
      }
      else if (i == 1)
      {
        rectTransform.localPosition = new Vector2(-canvas.rect.width / 4f + canvas.rect.width / 2f, canvas.rect.height / 4f);
      }
      else if (i == 2)
      {
        rectTransform.localPosition = new Vector2(-canvas.rect.width / 4f, canvas.rect.height / 4f + canvas.rect.height / 2f);
      }
      else
      {
        rectTransform.localPosition = new Vector2(-canvas.rect.width / 4f + canvas.rect.width / 2f, canvas.rect.height / 4f + canvas.rect.height / 2f);
      }
      proposal.Init(upgrade, () =>
      {
        upgraded.Add(i);
        if (upgraded.Count == upgrades.Count)
        {
          Time.timeScale = 1f;
          Destroy(gameObject);
        }
      });
    }
  }
}