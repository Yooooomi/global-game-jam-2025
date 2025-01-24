using UnityEngine;

public class BubbleHittable : MonoBehaviour, Hittable
{
    public void Hit(float damage)
    {
        Destroy(gameObject);
    }
}
