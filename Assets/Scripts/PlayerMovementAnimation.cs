using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
  private PlayerControls controls;
  private Animator animator;

  private void Start()
  {
    controls = GetComponentInParent<PlayerControls>();
    animator = GetComponent<Animator>();
  }

  private void Update()
  {
    // Animation
    if (controls.move != Vector2.zero)
    {
      animator.SetFloat("x", controls.move.x);
      animator.SetFloat("y", controls.move.y);
      animator.SetBool("walk", true);
    }
    else
    {
      animator.SetBool("walk", false);
    }
  }
}