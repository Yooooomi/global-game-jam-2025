using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class bubble : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D body;
    
    [SerializeField]
    private float changeVibeEverySec = 0.5f;
    [SerializeField]
    private float vibeLerp = 0.5f;
    [SerializeField]
    private float vibeIntensity = 100f;
    private float lastVibeDirChangeTime = 0;
    private Vector2 vibeDir;

    private void ResetVibeDir() {
        Debug.Log("Reset Vibe");
        Vector2 toDest = (target.transform.position - transform.position).normalized;

        // Random vibe from -90 up to 90 degree toward target
        float randomAngle = Random.Range(-90.0f, 90.0f);
        Vector2 newVibe = Quaternion.Euler(0, 0, randomAngle) * toDest;

        // Keep the current vibe into account by taking the mid point between the old vibe and new vibe
        newVibe = Vector2.Lerp(vibeDir.normalized, newVibe.normalized, vibeLerp);

        lastVibeDirChangeTime = Time.time;
        vibeDir = newVibe;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetVibeDir();
    }

    void Update() {
        if (Time.time > lastVibeDirChangeTime + changeVibeEverySec) {
            ResetVibeDir();
        }
    }

    void FixedUpdate()
    {
        Vector2 toDestination = (target.transform.position - transform.position).normalized;
        Vector2 movement = (toDestination + vibeDir * vibeIntensity).normalized;

        movement *= speed * Time.fixedDeltaTime;

        body.MovePosition(transform.position + (Vector3)movement);
    }
}
