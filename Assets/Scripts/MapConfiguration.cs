using UnityEngine;

public class MapConfiguration : MonoBehaviour
{
  public static MapConfiguration instance;

  public float minX;
  public float maxX;
  public float minY;
  public float maxY;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
  }

  public bool InMap(Vector3 position)
  {
    return position.x > minX && position.x < maxX && position.y > minY && position.y < maxY;
  }
}