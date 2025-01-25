using UnityEngine;

public class BubbleDrop : MonoBehaviour
{
  private BubbleHittable bubbleHittable;
  [SerializeField]
  private GameObject experienceOrbPrefab;

  private void Start()
  {
    bubbleHittable = GetComponent<BubbleHittable>();
    bubbleHittable.onBubbleDeath.AddListener(OnDeath);
  }

  private void OnDeath()
  {
    var go = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
    if (!go.TryGetComponent<ExperienceOrb>(out var experienceOrb))
    {
      return;
    }
    experienceOrb.Init(bubbleHittable.experience);
  }
}