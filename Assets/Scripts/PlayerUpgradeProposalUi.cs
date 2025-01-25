using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUpgradeProposalUi : MonoBehaviour
{
  private PlayerExperience playerExperience;
  [SerializeField]
  private PlayerUpgradeWholeProposal playerUpgradeProposal;
  private List<PlayerUpgrades> upgrades = new();

  private void Start()
  {
    playerExperience = GetComponent<PlayerExperience>();
    playerExperience.onLevelGainedEvent.AddListener(OnLevelGained);
  }

  private void OnPlayerJoined(PlayerInput input)
  {
    if (!input.gameObject.TryGetComponent<PlayerUpgrades>(out var upgrade))
    {
      return;
    }
    upgrades.Add(upgrade);
  }

  private void OnPlayerLeft(PlayerInput input)
  {
    if (!input.gameObject.TryGetComponent<PlayerUpgrades>(out var upgrade))
    {
      return;
    }
    upgrades.Remove(upgrade);
  }

  private void OnLevelGained()
  {
    playerUpgradeProposal.gameObject.SetActive(true);
    playerUpgradeProposal.Init(upgrades);
  }
}