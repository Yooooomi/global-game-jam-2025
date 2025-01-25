using UnityEngine;

public class PlayerMovementAnimation : MonoBehaviour
{
  private PlayerControls controls;
  private PlayerStats stats;
  private Animator animator;

  private void Start()
  {
    controls = GetComponentInParent<PlayerControls>();
    animator = GetComponent<Animator>();
    stats = GetComponentInParent<PlayerStats>();
    stats.onDeathEvent.AddListener(OnDeath);
    stats.onRespawnEvent.AddListener(OnRespawn);
  }

  private void OnDeath()
  {
    animator.SetBool("dead", true);
  }

  private void OnRespawn()
  {
    animator.SetBool("dead", false);
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