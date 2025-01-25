using UnityEngine;

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
    }
    return -1;
  }
}
