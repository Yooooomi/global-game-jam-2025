using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowRangeRegistry : MonoBehaviour
{
    private Collider2D range;

    private float lastInRangeFrameComputation = 0f;

    private List<Collider2D> cachedInRange = new List<Collider2D>();

    public List<Collider2D> GetInRange()
    {
        if (Time.time == lastInRangeFrameComputation)
        {
            return cachedInRange;
        }
        cachedInRange.Clear();
        range.OverlapCollider(new ContactFilter2D().NoFilter(), cachedInRange);
        return cachedInRange;
    }

    private void Start()
    {
        range = GetComponent<Collider2D>();
    }
}
