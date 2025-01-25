using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody2D rigidbody;

    [SerializeField]
    private GameObject lookAt;
    [SerializeField]
    private Animator animator;

    public float moveSpeed;

    private void Start()
    {
        controls = GetComponent<PlayerControls>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update() {

        float angle = Mathf.Atan2(controls.look.y, controls.look.x) * Mathf.Rad2Deg - 90f;
        lookAt.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Animation
        if (controls.move != Vector2.zero) {
            animator.SetFloat("x", controls.move.x);
            animator.SetFloat("y", controls.move.y);
            animator.SetBool("walk", true);
        } else {
            animator.SetBool("walk", false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + controls.move * moveSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
    }
}
