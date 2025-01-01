using UnityEngine;

public class MovingPlatform : MonoBehaviour, IDimensionAware
{
    [Header("Movement Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private bool isLoop = true;
    
    [Header("Dimension Settings")]
    [SerializeField] private int[] activeDimensions;

    private int currentWaypointIndex;
    private float waitTimer;
    private bool isWaiting;
    private bool isReversing;

    private void Start()
    {
        if (waypoints.Length < 2)
        {
            Debug.LogWarning("Platform needs at least 2 waypoints!");
            enabled = false;
            return;
        }

        transform.position = waypoints[0].position;
        DimensionManager.Instance.RegisterDimensionAware(this);
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                UpdateWaypointIndex();
            }
            return;
        }

        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if ((Vector2)transform.position == targetPosition)
        {
            isWaiting = true;
            waitTimer = waitTime;
        }
    }

    private void UpdateWaypointIndex()
    {
        if (isLoop)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else
        {
            if (isReversing)
            {
                currentWaypointIndex--;
                if (currentWaypointIndex <= 0)
                {
                    currentWaypointIndex = 0;
                    isReversing = false;
                }
            }
            else
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    currentWaypointIndex = waypoints.Length - 1;
                    isReversing = true;
                }
            }
        }
    }

    public void OnDimensionChanged(int dimensionId)
    {
        bool isActive = System.Array.Exists(activeDimensions, d => d == dimensionId);
        gameObject.SetActive(isActive);
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
            }
        }

        if (waypoints[waypoints.Length - 1] != null)
        {
            Gizmos.DrawWireSphere(waypoints[waypoints.Length - 1].position, 0.3f);
            if (isLoop && waypoints[0] != null)
            {
                Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
            }
        }
    }

    private void OnDestroy()
    {
        if (DimensionManager.Instance != null)
        {
            DimensionManager.Instance.UnregisterDimensionAware(this);
        }
    }
} 