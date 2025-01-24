using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersEvents : MonoBehaviour
{
    public List<PlayerInput> players { get; private set; } = new List<PlayerInput>();

    private void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("OnPlayerJoined");
        players.Add(input);
    }

    private void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log("OnPlayerLeft");
        players.Remove(input);
    }
}
