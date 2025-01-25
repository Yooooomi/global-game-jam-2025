using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerUpgrades upgrades;
    private PlayerControls controls;
    private Rigidbody2D rigidbody;

    public float moveSpeed;
    public float baseMoveSpeed;

    private void Start()
    {
        upgrades = GetComponent<PlayerUpgrades>();
        controls = GetComponent<PlayerControls>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private float GetMoveSpeed()
    {
        return baseMoveSpeed + upgrades.GetValueByKey("move_speed");
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + controls.move * GetMoveSpeed() * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
    }
}
