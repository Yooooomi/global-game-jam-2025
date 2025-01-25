using UnityEngine;

public class LevelUpSound : MonoBehaviour
{
  private PlayerExperience playerExperience;
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip audioClip;

  private void Start()
  {
    playerExperience = GetComponentInParent<PlayerExperience>();
    playerExperience.onLevelGainedEvent.AddListener(PlaySound);
    audioSource = GetComponent<AudioSource>();
  }

  private void PlaySound()
  {
    audioSource.PlayOneShot(audioClip);
  }
}
