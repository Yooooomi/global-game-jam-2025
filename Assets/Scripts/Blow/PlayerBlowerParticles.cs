using UnityEngine;

public class PlayerBlowerParticles : MonoBehaviour
{

  private PlayerBlower playerBlower;
  private ParticleSystem particleSystem;


  private void Start()
  {
    playerBlower = GetComponentInParent<PlayerBlower>();
    particleSystem = GetComponent<ParticleSystem>();
  }

  private void Update()
  {
    if (!playerBlower.blowing)
    {
      particleSystem.Stop();
    }

    if (!particleSystem.isPlaying)
    {
      particleSystem.Play();
      var main = particleSystem.main;
      main.startSpeed = 20f;
    }
  }
}
