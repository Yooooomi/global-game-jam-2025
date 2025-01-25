using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnLevelGainedEvent : UnityEvent { }

public class PlayerExperience : MonoBehaviour
{
  public List<int> levels = new();
  private int currentExperience = 0;
  public OnLevelGainedEvent onLevelGainedEvent = new();

  private void OnPlayerJoined(PlayerInput input)
  {
    var weapon = input.gameObject.GetComponent<PlayerWeapon>();
    weapon.onKillEvent.AddListener(OnKill);
  }

  private void Start()
  {
  }

  public void OnKill(int experience)
  {
    var level = GetLevel();
    currentExperience += experience;
    var nowLevel = GetLevel();
    if (level != nowLevel)
    {
      onLevelGainedEvent.Invoke();
    }
  }

  public int GetLevel()
  {
    int add = 0;
    for (int i = 0; i < levels.Count; i += 1)
    {
      add += levels[i];
      if (currentExperience < add)
      {
        return i;
      }
    }
    return levels.Count;
  }
}