using UnityEngine;

public class PlayerAim : MonoBehaviour
{
  private PlayerControls controls;
  private PlayerUpgrades playerUpgrades;

  private void Start()
  {
    controls = GetComponent<PlayerControls>();
    playerUpgrades = GetComponent<PlayerUpgrades>();
  }
}