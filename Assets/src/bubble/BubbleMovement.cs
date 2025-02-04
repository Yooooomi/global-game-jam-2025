using System;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public GameObject target;

    private float speed;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float refreshTargetEverySec = 2f;

    private float lastTargetRefreshTime = 0f;

    [SerializeField]
    private float changeVibeEverySec = 0.5f;
    [SerializeField]
    private float vibeIntensity = 100f;
    private float lastVibeDirChangeTime = 0;
    private Vector2 vibeDir;
    private Vector2 lastVibe;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    private void ResetVibeDir()
    {
        if (!target)
        {
            return;
        }
        Vector2 toDest = (target.transform.position - transform.position).normalized;

        // Random vibe from -90 up to 90 degree toward target
        float randomAngle = UnityEngine.Random.Range(-90.0f, 90.0f);
        Vector2 newVibe = Quaternion.Euler(0, 0, randomAngle) * toDest;

        lastVibeDirChangeTime = Time.time;
        lastVibe = vibeDir;
        vibeDir = newVibe.normalized;
    }

    private void UpdateTarget()
    {
        lastTargetRefreshTime = Time.time;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            return;
        }
        float distanceToClosestPlayer = float.MaxValue;
        GameObject closestPlayer = players[0];
        foreach (GameObject player in players)
        {
            if (!player.TryGetComponent<PlayerStats>(out var stats))
            {
                continue;
            }
            if (!stats.alive)
            {
                continue;
            }
            Vector2 distanceVec = player.transform.position - transform.position;
            float distance = Math.Abs(distanceVec.x) + Math.Abs(distanceVec.y);
            if (distance < distanceToClosestPlayer)
            {
                closestPlayer = player;
                distanceToClosestPlayer = distance;
            }
        }
        target = closestPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTarget();
        ResetVibeDir();
    }

    void Update()
    {
        if (Time.time > lastTargetRefreshTime + refreshTargetEverySec)
        {
            UpdateTarget();
        }
        if (Time.time > lastVibeDirChangeTime + changeVibeEverySec)
        {
            ResetVibeDir();
        }
    }

    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector2 toDestination = (target.transform.position - transform.position).normalized;

        // Convert vectors to angles
        float currentAngle = Mathf.Atan2(lastVibe.y, lastVibe.x) * Mathf.Rad2Deg;
        float targetAngle = Mathf.Atan2(vibeDir.y, vibeDir.x) * Mathf.Rad2Deg;

        // Calculate the proportion of time passed since the last reset
        float timeSinceLastReset = (Time.time - lastVibeDirChangeTime) / changeVibeEverySec;
        float smoothFactor = Mathf.Clamp01(timeSinceLastReset);

        // Smoothly interpolate the angles
        float resultAngle = Mathf.LerpAngle(currentAngle, targetAngle, smoothFactor);

        // Convert angle back to vector
        Vector2 frameVibe = new Vector2(Mathf.Cos(resultAngle * Mathf.Deg2Rad), Mathf.Sin(resultAngle * Mathf.Deg2Rad));

        Vector2 movement = (toDestination + frameVibe * vibeIntensity).normalized;

        movement *= speed * Time.fixedDeltaTime;

        body.MovePosition(transform.position + (Vector3)movement);
    }
}
