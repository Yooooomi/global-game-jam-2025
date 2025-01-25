using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUi : MonoBehaviour
{
  [SerializeField]
  private Slider slider;
  [SerializeField]
  private TMP_Text text;
  private PlayerExperience playerExperience;

  private void Start()
  {
    playerExperience = GameObject.Find("PlayerManager").GetComponent<PlayerExperience>();
  }

  private void Update()
  {
    slider.value = playerExperience.GetCurrentLevelAdvancement();
    text.text = (playerExperience.currentLevel + 1).ToString();
  }
}