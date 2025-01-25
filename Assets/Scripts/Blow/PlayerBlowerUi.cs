using UnityEngine;
using UnityEngine.UI;

public class PlayerBlowerUi : MonoBehaviour
{
  private Slider slider;
  private PlayerBlower playerBlower;

  private void Start()
  {
    slider = GetComponent<Slider>();
    playerBlower = GetComponentInParent<PlayerBlower>();
  }

  private void Update()
  {
    slider.value = playerBlower.energy / playerBlower.maxSeconds;
    if (playerBlower.energy == playerBlower.maxSeconds)
    {
      slider.enabled = false;
    }
    else
    {
      slider.enabled = true;
    }
  }
}
