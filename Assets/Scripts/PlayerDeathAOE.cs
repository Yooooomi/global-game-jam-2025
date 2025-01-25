using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAOE : MonoBehaviour
{
  public float blastTime;
  public float blastForce;
  public float forceDepletionPerMeter;

  private Collider2D collider;
  private PlayerStats stats;
  private bool aliveLastFrame;

  private void Start()
  {
    collider = GetComponent<Collider2D>();
    stats = GetComponentInParent<PlayerStats>();
    aliveLastFrame = true;
  }

  private IEnumerator DoBlast(List<Collider2D> hits)
  {
    var startTime = Time.time;
    while (true)
    {
      foreach (var item in hits)
      {
        if (item == null || !item.TryGetComponent<Blowable>(out var blowable))
        {
          continue;
        }
        var distance = Vector3.Distance(transform.position, item.transform.position);
        var force = blastForce - distance * forceDepletionPerMeter;
        if (force > 0)
        {
          blowable.Blow(transform.position, force * Time.deltaTime);
        }
      }
      if (startTime + blastTime < Time.time)
      {
        break;
      }
      yield return new WaitForEndOfFrame();
    }
  }

  private void Blast()
  {
    var hit = new List<Collider2D>();
    collider.OverlapCollider(new ContactFilter2D().NoFilter(), hit);
    StartCoroutine(DoBlast(hit));
  }

  private void Update()
  {
    if (aliveLastFrame && !stats.alive)
    {
      Blast();
    }
    aliveLastFrame = stats.alive;
  }
}