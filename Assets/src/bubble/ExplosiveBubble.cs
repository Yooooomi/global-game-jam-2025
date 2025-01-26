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
    private bool hasExploded = false;

    public void Awake() {
        GetComponent<BubbleHittable>().onBubbleDeath.AddListener(Explode);
    }

    public void Explode() {
        if (!shouldExplode || hasExploded) {
            return;
        }
        hasExploded = true;
        explosion.Explode(radius, /*damage=*/attack.damage, transform.position, /*damagePlayer=*/true);
    }
}
