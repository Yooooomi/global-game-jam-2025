using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnPlayerJoinedEvent : UnityEvent<PlayerInput> { }
public class OnPlayerLeftEvent : UnityEvent<PlayerInput> { }

public class PlayersEvents : MonoBehaviour
{
    public List<PlayerInput> players { get; private set; } = new List<PlayerInput>();
    public OnPlayerJoinedEvent onPlayerJoinedEvent = new();
    public OnPlayerLeftEvent onPlayerLeftEvent = new();

    private void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("OnPlayerJoined");
        players.Add(input);
        onPlayerJoinedEvent.Invoke(input);
    }

    private void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log("OnPlayerLeft");
        players.Remove(input);
        onPlayerLeftEvent.Invoke(input);
    }
}
