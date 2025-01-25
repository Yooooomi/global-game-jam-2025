using TMPro;
using UnityEngine;

public class PlayerUpgradeCard : MonoBehaviour
{
  [SerializeField]
  private TMP_Text title;
  [SerializeField]
  private TMP_Text description;
  [SerializeField]
  private TMP_Text level;

  public void Init(string title, string description, int level)
  {
    this.title.text = title;
    this.description.text = description;
    this.level.text = level.ToString();
  }
}