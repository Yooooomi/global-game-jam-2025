using TMPro;
using UnityEngine;

public class PlayerUpgradeCard : MonoBehaviour
{
  [SerializeField]
  private TMP_Text title;
  [SerializeField]
  private TMP_Text description;

  public void Init(string title, string description)
  {
    this.title.text = title;
    this.description.text = description;
  }
}