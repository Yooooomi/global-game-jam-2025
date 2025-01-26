using UnityEngine;

public class Rotating : MonoBehaviour
{
  public float speed;

  private void Update()
  {
    var currentRotation = transform.localRotation.eulerAngles.z;
    transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation + speed * Time.deltaTime);
  }
}