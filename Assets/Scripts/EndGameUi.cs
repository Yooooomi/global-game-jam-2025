using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnNewGameEvent : UnityEvent { }

public class EndGameUi : MonoBehaviour
{
  [SerializeField]
  private List<EndGamePlayerUi> uis;
  public OnNewGameEvent onNewGameEvent = new();

  public void Init(List<PlayerStats> stats)
  {
    var i = 0;
    for (; i < stats.Count; i += 1)
    {
      uis[i].gameObject.SetActive(true);
      uis[i].Init(stats[i].GetComponent<PlayerGameStats>());
    }
    for (; i < 4; i += 1)
    {
      uis[i].gameObject.SetActive(false);
    }

    foreach (var player in GameObject.Find("PlayerManager").GetComponent<PlayersEvents>().players)
    {
      player.actions.FindAction("UpgradeSecond", true).performed += OnNewGame;
    }
  }

  public void OnNewGame(InputAction.CallbackContext context)
  {
    foreach (var player in GameObject.Find("PlayerManager").GetComponent<PlayersEvents>().players)
    {
      player.actions.FindAction("UpgradeSecond", true).performed -= OnNewGame;
    }
    NewGame();
  }

  public void NewGame()
  {
    onNewGameEvent.Invoke();
  }
}