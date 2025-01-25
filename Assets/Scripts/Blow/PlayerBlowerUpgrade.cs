using UnityEngine;

public class PlayerBlowerUpgrade : MonoBehaviour
{
  private PolygonCollider2D polygonCollider;
  private PlayerBlower playerBlower;

  private void Start()
  {
    polygonCollider = GetComponent<PolygonCollider2D>();
    playerBlower = GetComponentInParent<PlayerBlower>();
  }

  private void HandleSize()
  {
    var angle = 30f;
    var size = playerBlower.GetBlowerRange();

    polygonCollider.points = new Vector2[] {
            Vector2.zero,
            Quaternion.Euler(0, 0, 90f) * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * size,
            Quaternion.Euler(0, 0, 90f) * new Vector2(Mathf.Cos(Mathf.Deg2Rad * -angle), Mathf.Sin(Mathf.Deg2Rad * -angle)) * size,
        };
  }

  private void Update()
  {
    HandleSize();
  }
}