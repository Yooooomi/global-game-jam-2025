using UnityEngine;

public class PlayLifecycleSound : MonoBehaviour
{
  [SerializeField]
  private RandomSound death;
  [SerializeField]
  private RandomSound respawn;

  private PlayerStats stats;

  private void Start()
  {
    stats = GetComponentInParent<PlayerStats>();
    stats.onDeathEvent.AddListener(OnDeath);
    stats.onRespawnEvent.AddListener(OnRespawn);
  }

  private void OnDeath()
  {
    death.PlayRandom();
  }

  private void OnRespawn()
  {
    respawn.PlayRandom();
  }
}