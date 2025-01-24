using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowRangeRegistry : MonoBehaviour
{
    public List<Collider2D> inRange { get; private set; } = new List<Collider2D>();
    private Collider2D range;

    private void Start()
    {
        range = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        range.OverlapCollider(new ContactFilter2D().NoFilter(), inRange);
    }
}
