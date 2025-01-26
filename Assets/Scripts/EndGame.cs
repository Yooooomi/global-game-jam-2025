using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
  private PlayersEvents playersEvents;
  private List<PlayerStats> playerStats = new();
  private bool ended = false;

  [SerializeField]
  private EndGameUi endGameUi;

  private void Start()
  {
    playersEvents = GameObject.Find("PlayerManager").GetComponent<PlayersEvents>();
    playersEvents.onPlayerJoinedEvent.AddListener(OnJoin);
    playersEvents.onPlayerLeftEvent.AddListener(OnLeave);
    endGameUi.onNewGameEvent.AddListener(NewGame);
  }

  private void OnJoin(PlayerInput playerInput)
  {
    playerStats.Add(playerInput.GetComponent<PlayerStats>());
  }

  private void OnLeave(PlayerInput playerInput)
  {
    playerStats.Remove(playerInput.GetComponent<PlayerStats>());
  }

  private void DoEndGame()
  {
    Time.timeScale = 0f;
    endGameUi.gameObject.SetActive(true);
    endGameUi.Init(playerStats);
  }

  private void Update()
  {
    if (ended == true)
    {
      return;
    }
    if (playerStats.Count == 0)
    {
      return;
    }
    foreach (var player in playerStats)
    {
      if (player.alive)
      {
        return;
      }
    }
    ended = true;
    DoEndGame();
  }

  public void NewGame()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(0);
  }
}