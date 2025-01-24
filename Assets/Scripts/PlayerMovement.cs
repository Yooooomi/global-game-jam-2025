using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody2D rigidbody;

    public float moveSpeed;

    private void Start()
    {
        controls = GetComponent<PlayerControls>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + controls.move * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
        rigidbody.MoveRotation(Mathf.Atan2(controls.look.y, controls.look.x) * Mathf.Rad2Deg - 90f);
    }
}
