using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Test : MonoBehaviour
{
    public GameObject[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (waypoints.Length == 0)
            return;

        // 현재 Waypoint로 이동
        Transform targetWaypoint = waypoints[currentWaypointIndex].transform;
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Waypoint에 도착하면 다음 Waypoint로 이동
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
