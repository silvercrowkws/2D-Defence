using System;
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
    /// 몬스터의 체력
    /// </summary>
    protected float hp = 30.0f;

    /// <summary>
    /// 어택 베이스
    /// </summary>
    //AttackBase attackBase;

    /// <summary>
    /// 몬스터 체력 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            if(hp != value)
            {
                hp = value;
                if(hp < 1)
                {
                    hp = 0;                     // 몬스터의 hp가 0이 된다면
                    monsterDieCount++;          // 죽은 몬스터의 숫자를 증가시키고
                    Debug.Log($"죽은 몬스터의 숫자 : {monsterDieCount}");
                    //IncrementMonsterDieCount();
                    onDie?.Invoke();            // 몬스터가 죽었다고 델리게이트로 알림(공격순위 리스트에서 빼기 위해)
                    //attackBase.attackList.Remove(this);

                    TurnEndProcess();
                    Destroy(gameObject);        // 게임 오브젝트 파괴
                    Debug.Log("사망");
                }
            }
        }
    }

    /// <summary>
    /// 죽은 몬스터의 숫자
    /// </summary>
    static int monsterDieCount = 0;

    /// <summary>
    /// 몬스터가 죽었다고 알리는 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 웨이포인트에 도착하면 대기하는 시간
    /// </summary>
    protected float waitTime = 1.0f;

    /// <summary>
    /// 웨이포인트의 개수
    /// </summary>
    int waypointCount;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

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

        turnManager = FindAnyObjectByType<TurnManager>();

        //attackBase = FindAnyObjectByType<AttackBase>();
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

        // 여기부터는 모든 웨이포인트를 순회했음

        monsterDieCount++;          // 죽은 몬스터의 숫자 증가
        Debug.Log($"죽은 몬스터의 숫자 : {monsterDieCount}");
        //IncrementMonsterDieCount();

        TurnEndProcess();           // 턴을 종료시켜야 하는지 확인
        Destroy(gameObject);        // 게임 오브젝트 파괴


        // 이후에 성의 체력을 감소 시키는 부분 필요
    }

    /// <summary>
    /// 턴을 종료시켜야 하는지 확인하는 함수
    /// </summary>
    void TurnEndProcess()
    {
        if (monsterDieCount == enemySpawner.maxMonsterCount)     // 죽은 몬스터의 숫자가 최대 몬스터 숫자이면
        {
            monsterDieCount = 0;                                // 죽은 몬스터의 숫자 초기화
            turnManager.OnTurnEnd2();
        }
    }

    /*/// <summary>
    /// 죽은 몬스터의 숫자를 증가시키는 함수
    /// </summary>
    protected virtual void IncrementMonsterDieCount()
    {
        monsterDieCount++;
        Debug.Log($"Monster died, total count : {monsterDieCount}");
    }*/
}
