using UnityEngine;
using System.Collections.Generic;

public class GuardAI : MonoBehaviour
{
    // ========== COOPERATIVE ALERT SYSTEM ==========
    public static bool globalAlert = false;
    public static Vector2 sharedPlayerPos;
    public static float lastSeenTime = 0f;
    public static List<GuardAI> allGuards = new List<GuardAI>();
    public static float alertTimeout = 5f; // Alert hilang setelah 5 detik tidak ada yang lihat
    // ===============================================

    public enum State { Patrol, Chase, Search, Surround }
    public State currentState = State.Patrol;

    [Header("Reference")]
    public Transform player;
    public Transform[] waypoints;

    [Header("Settings")]
    public float speed = 3f;
    public float chaseDistance = 5f;
    public float loseDistance = 7f;
    public float searchTime = 3f;
    public float surroundDistance = 3f; // Jarak untuk mengepung
    public LayerMask wallMask;

    private int wpIndex = 0;
    private float searchTimer = 0f;
    private Rigidbody2D rb;
    private Vector2 surroundPosition; // Posisi untuk mengepung
    private int guardIndex; // Index guard ini dalam formasi

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Register guard ke sistem cooperative
        if (!allGuards.Contains(this))
        {
            guardIndex = allGuards.Count;
            allGuards.Add(this);
        }
    }

    void OnDestroy()
    {
        allGuards.Remove(this);

        // Reset 
        if (allGuards.Count == 0)
        {
            globalAlert = false;
            sharedPlayerPos = Vector2.zero;
            lastSeenTime = 0f;
        }
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);


        // COOPERATIVE: Check apakah ada alert dari guard lain

        if (globalAlert && Time.time - lastSeenTime < alertTimeout)
        {
            if (currentState == State.Patrol)
            {
                currentState = State.Surround;
                CalculateSurroundPosition();
            }
        }
        else if (globalAlert && Time.time - lastSeenTime >= alertTimeout)
        {
            // Alert timeout, semua guard kembali patrol
            globalAlert = false;
        }

        // reflex agent 
        if (dist < chaseDistance && CanSeePlayer())
        {
            currentState = State.Chase;

            // Broadcast ke semua guard
            globalAlert = true;
            sharedPlayerPos = player.position;
            lastSeenTime = Time.time;

            // Update semua guard lain untuk surround
            NotifyOtherGuards();
        }

        // fsm
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase(dist);
                break;

            case State.Search:
                Search();
                break;

            case State.Surround:
                Surround();
                break;
        }
    }


    // PATROL (Path Follow)

    void Patrol()
    {
        Transform target = waypoints[wpIndex];
        MoveTowards(target.position);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            wpIndex = (wpIndex + 1) % waypoints.Length;
        }
    }


    // CHASE PLAYER (Direct pursuit)

    void Chase(float dist)
    {
        if (CanSeePlayer())
        {
            // Update posisi player untuk guard lain
            sharedPlayerPos = player.position;
            lastSeenTime = Time.time;

            MoveTowards(player.position);
        }
        else
        {
            // Kehilangan sight, pindah ke surround mode
            currentState = State.Surround;
            CalculateSurroundPosition();
            return;
        }

        // Terlalu jauh dan tidak bisa lihat
        if (dist > loseDistance && !CanSeePlayer())
        {
            currentState = State.Search;
            searchTimer = 0f;
        }
    }


    // SURROUND MODE (Koordinasi mengepung)

    void Surround()
    {
        // Update posisi surround berdasarkan posisi player terbaru
        if (Time.time - lastSeenTime < 0.5f) // Player baru dilihat
        {
            CalculateSurroundPosition();
        }

        // Cek apakah player terlihat lagi
        if (CanSeePlayer() && Vector2.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = State.Chase;
            sharedPlayerPos = player.position;
            lastSeenTime = Time.time;
            NotifyOtherGuards();
            return;
        }

        // Gerak ke posisi surround
        MoveTowards(surroundPosition);

        // Sudah sampai posisi surround tapi player tidak ketemu
        if (Vector2.Distance(transform.position, surroundPosition) < 0.5f)
        {
            if (Time.time - lastSeenTime > 2f)
            {
                currentState = State.Search;
                searchTimer = 0f;
            }
        }
    }


    // SEARCH MODE

    void Search()
    {
        searchTimer += Time.deltaTime;
        rb.velocity = Vector2.zero;

        // Jika player terlihat saat search
        if (CanSeePlayer() && Vector2.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = State.Chase;
            globalAlert = true;
            sharedPlayerPos = player.position;
            lastSeenTime = Time.time;
            NotifyOtherGuards();
            return;
        }

        if (searchTimer > searchTime)
        {
            // Cari waypoint terdekat
            wpIndex = FindNearestVisibleWaypoint();

            // Reset alert jika semua guard sudah selesai search
            if (!IsAnyGuardChasing())
            {
                globalAlert = false;
            }

            currentState = State.Patrol;
        }
    }


    // CALCULATE SURROUND POSITION

    void CalculateSurroundPosition()
    {
        int totalGuards = allGuards.Count;
        float angleStep = 360f / totalGuards;
        float angle = angleStep * guardIndex;

        // Posisi melingkar di sekitar player
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(
            Mathf.Cos(rad) * surroundDistance,
            Mathf.Sin(rad) * surroundDistance
        );

        surroundPosition = sharedPlayerPos + offset;
    }


    // NOTIFY OTHER GUARDS

    void NotifyOtherGuards()
    {
        foreach (GuardAI guard in allGuards)
        {
            if (guard != this && guard.currentState == State.Patrol)
            {
                guard.currentState = State.Surround;
                guard.CalculateSurroundPosition();
            }
        }
    }


    // CHECK IF ANY GUARD IS CHASING

    bool IsAnyGuardChasing()
    {
        foreach (GuardAI guard in allGuards)
        {
            if (guard.currentState == State.Chase || guard.currentState == State.Surround)
                return true;
        }
        return false;
    }


    // MOVEMENT + obstacle avoid

    void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        dir = AvoidObstacle(dir);
        rb.velocity = dir * speed;
    }

    Vector2 AvoidObstacle(Vector2 desiredDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, desiredDir, 0.8f, wallMask);

        if (hit.collider == null)
            return desiredDir;

        Vector2 left = new Vector2(-desiredDir.y, desiredDir.x).normalized;
        if (!Physics2D.Raycast(transform.position, left, 0.6f, wallMask))
            return left;

        Vector2 right = new Vector2(desiredDir.y, -desiredDir.x).normalized;
        if (!Physics2D.Raycast(transform.position, right, 0.6f, wallMask))
            return right;

        return Vector2.zero;
    }


    // LINE OF SIGHT

    bool CanSeePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, wallMask);

        if (hit.collider != null)
            return false;

        return true;
    }


    // WAYPOINT TERDEKAT TANPA TEMBOK

    int FindNearestVisibleWaypoint()
    {
        float bestDist = Mathf.Infinity;
        int bestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform w = waypoints[i];
            Vector2 direction = (w.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, w.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, wallMask);
            if (hit.collider != null)
                continue;

            if (distance < bestDist)
            {
                bestDist = distance;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            // Line of sight
            Gizmos.color = CanSeePlayer() ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);

            // Surround position
            if (currentState == State.Surround)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(surroundPosition, 0.3f);
                Gizmos.DrawLine(transform.position, surroundPosition);
            }
        }
    }
}