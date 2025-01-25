using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUpgradeProposal : MonoBehaviour
{
  private PlayerUpgrades upgrades;
  [SerializeField]
  private GameObject selecting;
  [SerializeField]
  private GameObject selected;
  [SerializeField]
  private List<PlayerUpgradeCard> cards = new();
  [SerializeField]
  private Image playerImage;
  private PlayerInput playerInput;
  private List<string> proposedKeys;
  private Action OnUpgraded;

  public bool Init(PlayerUpgrades upgrades, Action onUpgraded)
  {
    OnUpgraded = onUpgraded;
    this.upgrades = upgrades;
    var proposed = upgrades.GetNextUpgrades();

    if (proposed.Count == 0)
    {
      return false;
    }

    selected.SetActive(false);
    selecting.SetActive(true);

    proposedKeys = proposed.Select(e => e.key).ToList();

    int i = 0;
    for (i = 0; i < proposed.Count; i += 1)
    {
      var thisProposed = proposed[i];
      cards[i].gameObject.SetActive(true);
      cards[i].Init(thisProposed.name, upgrades.GetDescriptionByKey(thisProposed.key), upgrades.GetLevelByKey(thisProposed.key) + 1);
    }
    for (; i < 3; i += 1)
    {
      cards[i].gameObject.SetActive(false);
    }

    var input = upgrades.gameObject.GetComponent<PlayerInput>();
    playerInput = input;
    SetupKeys();
    playerImage.sprite = upgrades.GetComponentInChildren<PlayerSkin>().GetPresentation();
    return true;
  }

  private void SetupKeys()
  {
    if (proposedKeys.Count > 0)
    {
      playerInput.actions.FindAction("UpgradeFirst", true).performed += OnFirst;
    }
    if (proposedKeys.Count > 1)
    {
      playerInput.actions.FindAction("UpgradeSecond", true).performed += OnSecond;
    }
    if (proposedKeys.Count > 2)
    {
      playerInput.actions.FindAction("UpgradeThird", true).performed += OnThird;
    }
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
    selected.SetActive(true);
    selecting.SetActive(false);
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
