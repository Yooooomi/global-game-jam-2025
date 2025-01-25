using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUi : MonoBehaviour
{
  private PlayerStats stats;
  private Slider slider;

  private void Start()
  {
    stats = GetComponentInParent<PlayerStats>();
    slider = GetComponent<Slider>();
  }

  private void Update()
  {
    slider.value = stats.health / stats.GetMaxHealth();
  }
}
