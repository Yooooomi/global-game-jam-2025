using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnLevelGainedEvent : UnityEvent { }

public class PlayerExperience : MonoBehaviour
{
  public int additionalXpPerLevel;
  public int currentLevel = 0;
  private int currentExperience = 0;
  public OnLevelGainedEvent onLevelGainedEvent = new();

  private void OnPlayerJoined(PlayerInput input)
  {
    var weapon = input.gameObject.GetComponent<PlayerWeapon>();
    weapon.onKillEvent.AddListener(OnKill);
  }

  private bool CheckLevelPassed()
  {
    var xpNeededForNextLevel = (currentLevel + 1) * additionalXpPerLevel;
    if (currentExperience >= xpNeededForNextLevel)
    {
      currentLevel += 1;
      currentExperience -= xpNeededForNextLevel;
      return true;
    }
    return false;
  }

  public void OnKill(int experience)
  {
    currentExperience += experience;
    var passedLevel = CheckLevelPassed();
    if (passedLevel)
    {
      onLevelGainedEvent.Invoke();
    }
  }

  public float GetCurrentLevelAdvancement()
  {
    var xpNeededForNextLevel = (currentLevel + 1) * additionalXpPerLevel;
    return (float)currentExperience / xpNeededForNextLevel;
  }
}