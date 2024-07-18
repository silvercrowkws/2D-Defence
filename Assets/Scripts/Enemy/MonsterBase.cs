using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    /// <summary>
    /// 웨이포인트
    /// </summary>
    Transform[] waypoints;

    /// <summary>
    /// 적 스폰하는 오브젝트
    /// </summary>
    EnemySpawner enemySpawner;

    /// <summary>
    /// 현재 목표 웨이포인트 인덱스
    /// </summary>
    int currentWaypointIndex = 0;

    /// <summary>
    /// 이동 속도
    /// </summary>
    protected float moveSpeed = 2.0f;

    /// <summary>
    /// 웨이포인트에 도착하면 대기하는 시간
    /// </summary>
    protected float waitTime = 1.0f;

    /// <summary>
    /// 웨이포인트의 개수
    /// </summary>
    int waypointCount;

    protected virtual void Start()
    {
        enemySpawner = FindAnyObjectByType<EnemySpawner>();

        waypointCount = enemySpawner.transform.childCount;      // enemySpawner의 자식 개수

        waypoints = new Transform[waypointCount];

        for (int i = 0; i < waypointCount; i++)
        {
            waypoints[i] = enemySpawner.transform.GetChild(i).transform;
        }

        // 처음 웨이포인트로 이동 시작
        if (waypoints.Length > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    /// <summary>
    /// 다음 웨이포인트로 움직이는 코루틴
    /// </summary>
    IEnumerator MoveToNextWaypoint()
    {
        while (currentWaypointIndex < waypointCount)
        {
            if (waypoints.Length == 0)
                yield break;

            // 현재 목표 웨이포인트 위치
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;

            // 몬스터가 목표 위치에 도달할 때까지 이동
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 다음 웨이포인트로 인덱스 증가 (순환하게 하기 위해 % 사용)
            //currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            currentWaypointIndex++;

            // 잠시 대기
            yield return new WaitForSeconds(waitTime);
        }

        // 모든 웨이포인트를 순회한 후, 게임 오브젝트를 파괴
        Destroy(gameObject);
    }
}
