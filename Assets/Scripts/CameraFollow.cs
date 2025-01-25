using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float moveRigidity;
    private PlayersEvents playersEvents;
    private Camera camera;
    public float padding;
    public float minOrthographicSize;

    private void Start()
    {
        playersEvents = GameObject.Find("PlayerManager").GetComponent<PlayersEvents>();
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (playersEvents.players.Count == 0)
        {
            return;
        }

        // Handling position
        Vector3 sum = Vector3.zero;

        foreach (var player in playersEvents.players)
        {
            sum += player.transform.position;
        }

        var average = sum / playersEvents.players.Count;

        var target = Vector3.Lerp(transform.position, average, moveRigidity * Time.deltaTime);
        target.z = -10f;

        transform.position = target;

        // Handling zoom
        var bounds = new Bounds(playersEvents.players[0].transform.position, Vector3.zero);

        foreach (var player in playersEvents.players)
        {
            bounds.Encapsulate(player.transform.position);
        }

        float verticalSize = bounds.extents.y + padding;
        float horizontalSize = (bounds.extents.x + padding) / camera.aspect;
        float requiredSize = Mathf.Max(verticalSize, horizontalSize);

        camera.orthographicSize = Mathf.Max(requiredSize, minOrthographicSize);
    }
}
