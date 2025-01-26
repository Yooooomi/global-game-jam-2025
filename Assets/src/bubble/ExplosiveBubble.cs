using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBubble : MonoBehaviour
{
    [SerializeField]
    private Explosion explosion;
    [SerializeField]
    private BubbleAttack attack;
    [SerializeField]
    private float radius = 2f;

    public bool shouldExplode = false;

    public void Awake() {
        GetComponent<BubbleHittable>().onBubbleDeath.AddListener(Explode);
    }

    public void Explode() {
        if (!shouldExplode) {
            return;
        }
        explosion.Explode(radius, /*damage=*/attack.damage, transform.position, /*damagePlayer=*/true);
    }
}
