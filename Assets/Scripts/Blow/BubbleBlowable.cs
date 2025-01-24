using UnityEngine;

public class BubbleBlowable : MonoBehaviour, Blowable
{
    public void Blow(Vector3 from, float force)
    {
        var direction = (transform.position - from).normalized * force;
        transform.position += direction;
    }
}
