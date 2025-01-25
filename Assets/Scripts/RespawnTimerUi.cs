using TMPro;
using UnityEngine;

public class RespawnTimerUi : MonoBehaviour
{
  private PlayerStats stats;
  private TMP_Text text;

  private void Start()
  {
    stats = GetComponentInParent<PlayerStats>();
    text = GetComponent<TMP_Text>();
  }

  private void Update()
  {
    if (stats.alive)
    {
      text.enabled = false;
      return;
    }
    text.enabled = true;
    text.text = Mathf.FloorToInt(stats.cooldownUntilAlive).ToString();
  }
}