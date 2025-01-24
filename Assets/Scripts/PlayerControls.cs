using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    private PlayerInput input;

    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }

    private bool isKeyboardAndMouse = false;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        isKeyboardAndMouse = input.GetDevice<Mouse>() != null;
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void Update()
    {
        var mousePosition = Mouse.current.position.value;
        Vector2 playerViewportPoint = camera.WorldToViewportPoint(transform.position);
        Vector2 mouseViewportPoint = camera.ScreenToViewportPoint(mousePosition);
        var diff = mouseViewportPoint - playerViewportPoint;
        look = diff;
    }

    private void OnLook(InputValue value)
    {
        if (isKeyboardAndMouse) {
            return;
        }
        look = value.Get<Vector2>();
    }
}
