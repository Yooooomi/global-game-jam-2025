using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    private PlayerInput input;

    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }
    public bool blow { get; private set; }
    public bool shoot { get; private set; }

    private bool isKeyboardAndMouse = false;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var firstDevice = input.devices[0];
        isKeyboardAndMouse = firstDevice != null && (firstDevice.name == "Mouse" || firstDevice.name == "Keyboard");
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void Update()
    {
        if (!isKeyboardAndMouse)
        {
            return;
        }
        var mousePosition = Mouse.current.position.value;
        Vector2 playerViewportPoint = camera.WorldToScreenPoint(transform.position);
        var diff = mousePosition - playerViewportPoint;
        look = diff;
    }

    private void OnLook(InputValue value)
    {
        if (isKeyboardAndMouse)
        {
            return;
        }
        look = value.Get<Vector2>();
    }

    private void OnBlow(InputValue value)
    {
        blow = value.Get<float>() > 0.5f;
    }

    private void OnShoot(InputValue value)
    {
        shoot = value.Get<float>() > 0.5f;
    }
}
