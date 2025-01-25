using UnityEngine;
using UnityEngine.Events;

public class OnDeathEvent : UnityEvent { }
public class OnRespawnEvent : UnityEvent { }

public class PlayerStats : MonoBehaviour, Hittable
{
  private PlayerUpgrades upgrades;

  public float health { get; private set; }
  public float baseMaxHealth;
  public float healthResplenishPerSeconds;
  public float healthResplenishCooldown;
  public bool alive;
  public float cooldownUntilAlive { get; private set; }
  public float respawnTime;
  private float lastHit;

  public OnDeathEvent onDeathEvent = new();
  public OnRespawnEvent onRespawnEvent = new();

  public float GetMaxHealth()
  {
    return baseMaxHealth + upgrades.GetValueByKey("max_health");
  }

  private void Start()
  {
    alive = true;
    health = baseMaxHealth;
    upgrades = GetComponent<PlayerUpgrades>();
  }

  private void ResplenishHealth()
  {
    if (!alive)
    {
      return;
    }
    if (lastHit + healthResplenishCooldown > Time.time)
    {
      return;
    }
    if (health == GetMaxHealth())
    {
      return;
    }
    health += healthResplenishPerSeconds * Time.deltaTime;
    if (health > GetMaxHealth())
    {
      health = GetMaxHealth();
    }
  }

  private void HandleRespawn()
  {
    if (alive)
    {
      return;
    }
    cooldownUntilAlive -= Time.deltaTime;
    if (cooldownUntilAlive < 0)
    {
      alive = true;
      health = GetMaxHealth() / 2f;
      onRespawnEvent.Invoke();
    }
  }

  private void Update()
  {
    ResplenishHealth();
    HandleRespawn();
  }

  public int Hit(float damage)
  {
    if (!alive)
    {
      return -1;
    }
    health -= damage;
    lastHit = Time.time;
    if (health < 0)
    {
      alive = false;
      health = 0;
      cooldownUntilAlive = respawnTime;
      onDeathEvent.Invoke();
    }
    return -1;
  }
}
