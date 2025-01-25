using UnityEngine;

public class CanonRotation : MonoBehaviour
{
  private PlayerControls controls;

  private void Start()
  {
    controls = GetComponentInParent<PlayerControls>();
  }

  private void Update()
  {
    float angle = Mathf.Atan2(controls.look.y, controls.look.x) * Mathf.Rad2Deg - 90f;
    transform.rotation = Quaternion.Euler(0, 0, angle);
  }
}