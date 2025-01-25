using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUpgradeProposal : MonoBehaviour
{
  private PlayerUpgrades upgrades;
  [SerializeField]
  private List<PlayerUpgradeCard> cards = new();
  private PlayerInput playerInput;
  private List<string> proposedKeys;
  private Action OnUpgraded;

  public void Init(PlayerUpgrades upgrades, Action onUpgraded)
  {
    OnUpgraded = onUpgraded;
    this.upgrades = upgrades;
    var proposed = upgrades.GetNextUpgrades();
    proposedKeys = proposed.Select(e => e.key).ToList();
    for (int i = 0; i < proposed.Count; i += 1)
    {
      var thisProposed = proposed[i];
      cards[i].Init(thisProposed.name, upgrades.GetDescriptionByKey(thisProposed.key));
    }
    var input = upgrades.gameObject.GetComponent<PlayerInput>();
    playerInput = input;
    SetupKeys();
  }

  private void SetupKeys()
  {
    playerInput.actions.FindAction("UpgradeFirst", true).performed += OnFirst;
    playerInput.actions.FindAction("UpgradeSecond", true).performed += OnSecond;
    playerInput.actions.FindAction("UpgradeThird", true).performed += OnThird;
  }

  private void UnsetupKeys()
  {
    playerInput.actions.FindAction("UpgradeFirst", true).performed -= OnFirst;
    playerInput.actions.FindAction("UpgradeSecond", true).performed -= OnSecond;
    playerInput.actions.FindAction("UpgradeThird", true).performed -= OnThird;
  }

  private void End()
  {
    UnsetupKeys();
    OnUpgraded();
  }

  public void OnFirst(InputAction.CallbackContext context)
  {
    upgrades.UpgradeByKey(proposedKeys[0]);
    End();
  }

  public void OnSecond(InputAction.CallbackContext context)
  {
    upgrades.UpgradeByKey(proposedKeys[1]);
    End();
  }

  public void OnThird(InputAction.CallbackContext context)
  {
    upgrades.UpgradeByKey(proposedKeys[2]);
    End();
  }
}
