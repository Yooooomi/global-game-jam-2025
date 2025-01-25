using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Drop
{
  public float rate;
  public GameObject prefab;
}

public class BubbleDrop : MonoBehaviour
{
  private BubbleHittable bubbleHittable;
  [SerializeField]
  private GameObject experienceOrbPrefab;
  [SerializeField]
  private List<Drop> drops;

  private void Start()
  {
    bubbleHittable = GetComponent<BubbleHittable>();
    bubbleHittable.onBubbleDeath.AddListener(OnDeath);
    drops = drops.OrderBy(e => e.rate).ToList();
  }

  private void OnDeath()
  {
    var go = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
    if (!go.TryGetComponent<ExperienceOrb>(out var experienceOrb))
    {
      return;
    }
    experienceOrb.Init(bubbleHittable.experience);

    foreach (var drop in drops)
    {
      var probability = Random.Range(0f, 1f);
      if (probability > drop.rate)
      {
        continue;
      }
      Instantiate(drop.prefab, transform.position, Quaternion.identity);
      return;
    }
  }
}