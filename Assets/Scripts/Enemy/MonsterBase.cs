using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

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
    /// 몬스터가 죽었을 때 주는 돈
    /// </summary>
    protected float dieMoney = 1.0f;

    /// <summary>
    /// 최대 체력
    /// </summary>
    protected float maxHP = 100.0f;

    /// <summary>
    /// 몬스터의 현재 체력
    /// </summary>
    protected float currentHp = 30.0f;

    /// <summary>
    /// 어택 베이스
    /// </summary>
    //AttackBase attackBase;

    /// <summary>
    /// 몬스터 체력 프로퍼티
    /// </summary>
    public float HP
    {
        get => currentHp;
        set
        {
            if(currentHp != value)
            {
                //currentHp = value;
                currentHp = Mathf.Clamp(value, 0, maxHP);
                if(currentHp < 1)
                {
                    currentHp = 0;                     // 몬스터의 hp가 0이 된다면
                    monsterDieCount++;          // 죽은 몬스터의 숫자를 증가시키고
                    Debug.Log($"죽은 몬스터의 숫자 : {monsterDieCount}");

                    gameManager.Money += dieMoney;      // 돈 증가

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

    /// <summary>
    /// 웨이포인트를 모두 순회한 몬스터의 숫자(문에 부딪힌 몬스터의 숫자)
    /// </summary>
    static int doorArriveMonster;

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;

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

        // 여기에 문에 도착한 적을 누적 시키는 부분 필요
        doorArriveMonster++;
        Debug.Log($"문에 도착한 적 : {doorArriveMonster}");
        gameManager.DoorLife--;

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
        Debug.Log("TurnEndProcess 실행");
        // 문에 도착한 몬스터가 최대가 아니고(10마리), 죽은 몬스터의 숫자가 최대 몬스터 숫자이면
        if (doorArriveMonster < enemySpawner.maxMonsterCount  && monsterDieCount == enemySpawner.maxMonsterCount)
        {
            monsterDieCount = 0;                                        // 죽은 몬스터의 숫자 초기화
            if(turnManager.turnNumber != turnManager.endTurnNumber)     // 현재 턴이 마지막 웨이브가 아니면
            {
                Debug.Log($"현재 턴 : {turnManager.turnNumber}");
                Debug.Log($"턴 종료 웨이브 : {turnManager.endTurnNumber}");

                turnManager.OnTurnEnd2();
            }
            else
            {
                gameManager.GameState = GameState.GameOver;     // 게임 상태를 Over로 변경
                turnManager.OnTurnOver(doorArriveMonster);      // 새로운 턴이 시작되지 않게 턴 종료
                doorArriveMonster = 0;                          // 초기화
            }
        }

        // 문에 도착한 몬스터가 최대이다(10마리)
        else if(doorArriveMonster >= enemySpawner.maxMonsterCount)
        {
            gameManager.GameState = GameState.GameOver;     // 게임 상태를 Over로 변경
            turnManager.OnTurnOver(doorArriveMonster);      // 새로운 턴이 시작되지 않게 턴 종료
            Debug.Log("문에 도착한 몬스터 최대");
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
