using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    /*/// <summary>
    /// enemyTilemap을 연결할 변수
    /// </summary>
    public Tilemap enemyTilemap;

    /// <summary>
    /// cyclopsTiles 타일을 연결할 변수
    /// </summary>
    public TileBase cyclopsTiles;

    /// <summary>
    /// ghostTiles 타일을 연결할 배열
    /// </summary>
    public TileBase[] ghostTiles;

    /// <summary>
    /// mouseTiles 타일을 연결할 배열
    /// </summary>
    public TileBase[] mouseTiles;

    /// <summary>
    /// spiderTiles 타일을 연결할 배열
    /// </summary>
    public TileBase[] spiderTiles;*/

    //GameObject[] waypoints;

    //public Sprite monsterSprite;

    /// <summary>
    /// 스폰할 위치
    /// </summary>
    //Vector3Int spawnerPosition;

    /// <summary>
    /// 생성된 몬스터의 숫자
    /// </summary>
    int monsterCount = 0;

    /// <summary>
    /// 최대 몬스터 숫자
    /// </summary>
    public int maxMonsterCount = 10;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    // 몬스터 프리팹
    public CyclopsMonster cyclopsPrefab;
    public MouseMonster_1 mouse_1Prefab;
    public MouseMonster_2 mouse_2Prefab;
    public GhostMonster_1 ghost_1Prefab;
    public GhostMonster_2 ghost_2Prefab;
    public SpiderMonster_1 spider_1Prefab;
    public SpiderMonster_2 spider_2Prefab;
    public LichMonster lichPrefab;

    /// <summary>
    /// 생성된 몬스터가 쌓일 장소
    /// </summary>
    GameObject monsterRepository;

    /// <summary>
    /// 몬스터 스폰 간격
    /// </summary>
    float spawnDelay = 0;

    private void Start()
    {

        monsterRepository = GameObject.Find("MonsterRepository");
        if (monsterRepository == null)
        {
            Debug.LogError("MonsterRepository를 찾을 수 없음");
        }
        else
        {
            DontDestroyOnLoad(monsterRepository);
            Debug.Log("MonsterRepository 초기화됨");
        }

        //DontDestroyOnLoad(monsterRepository);
        turnManager = FindAnyObjectByType<TurnManager>();
        if (turnManager != null)
        {
            turnManager.onTurnStart += OnTurnStartFC;
            Debug.Log("onTurnStart 이벤트 구독됨");
        }
        else
        {
            Debug.LogError("TurnManager를 찾을 수 없음");
        }

        //turnManager.onTurnStart += OnTurnStartFC;
    }

    /*private void OnEnable()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        if (turnManager != null)
        {
            turnManager.onTurnStart += OnTurnStartFC;
            Debug.Log("onTurnStart 이벤트 구독됨");
        }
        else
        {
            Debug.LogError("TurnManager를 찾을 수 없음");
        }
    }*/

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.onTurnStart -= OnTurnStartFC;
            Debug.Log("onTurnStart 이벤트 구독 해제됨");
        }
    }

    /// <summary>
    /// 몬스터를 스폰시키는 함수
    /// </summary>
    void SpawnerEnemy()
    {
        //monsterCount++;
        //Debug.Log($"{monsterCount} 마리째 몬스터 스폰");

        int cycle = (turnManager.turnNumber - 1) / 10;                  // 몬스터 주기 (0: 1-10턴, 1: 11-20턴)
        int turnInCycle = (turnManager.turnNumber - 1) % 10 + 1;        // 현재 주기 내의 턴 번호 (1-10)

        switch (turnInCycle)
        {
            case 1:
            case 2:
                if(cycle == 0)          // 1사이클이면
                {
                    // 기본 쥐 몬스터 소환
                    spawnDelay = 0.2f;
                    MouseMonster_1 mm1 = Instantiate(mouse_1Prefab, transform.position, Quaternion.identity);
                    mm1.name = $"Mouse1_{monsterCount}";
                    mm1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 쥐 몬스터 소환
                    spawnDelay = 0.2f;
                    MouseMonster_2 mm2 = Instantiate(mouse_2Prefab, transform.position, Quaternion.identity);
                    mm2.name = $"Mouse2_{monsterCount}";
                    mm2.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 3:
            case 4:
                if (cycle == 0)         // 1사이클이면
                {
                    // 기본 거미 몬스터 소환
                    spawnDelay = 0.2f;
                    SpiderMonster_1 sm1 = Instantiate(spider_1Prefab, transform.position, Quaternion.identity);
                    sm1.name = $"Spider1_{monsterCount}";
                    sm1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 거미 몬스터 소환
                    spawnDelay = 0.2f;
                    SpiderMonster_2 sm2 = Instantiate(spider_2Prefab, transform.position, Quaternion.identity);
                    sm2.name = $"Spider2_{monsterCount}";
                    sm2.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 5:
            case 6:
                if(cycle == 0)          // 1사이클이면
                {
                    // 기본 유령 몬스터 소환
                    spawnDelay = 1.0f;
                    GhostMonster_1 gm1 = Instantiate(ghost_1Prefab, transform.position, Quaternion.identity);
                    gm1.name = $"Ghost1_{monsterCount}";
                    gm1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 유령 몬스터 소환
                    spawnDelay = 1.0f;
                    GhostMonster_2 gm2 = Instantiate(ghost_2Prefab, transform.position, Quaternion.identity);
                    gm2.name = $"Ghost2_{monsterCount}";
                    gm2.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 7:
                if(cycle == 0)          // 1사이클이면
                {
                    // 기본 쥐 몬스터, 기본 거미 몬스터 소환
                    spawnDelay = 0.2f;
                    MouseMonster_1 mm1_1 = Instantiate(mouse_1Prefab, transform.position, Quaternion.identity);
                    mm1_1.name = $"Mouse1_{monsterCount}";
                    mm1_1.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    SpiderMonster_1 sm1_1 = Instantiate(spider_1Prefab, transform.position, Quaternion.identity);
                    sm1_1.name = $"Spider1_{monsterCount}";
                    sm1_1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 쥐 몬스터, 강화 거미 몬스터 소환
                    spawnDelay = 0.2f;
                    MouseMonster_2 mm2_1 = Instantiate(mouse_2Prefab, transform.position, Quaternion.identity);
                    mm2_1.name = $"Mouse2_{monsterCount}";
                    mm2_1.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    SpiderMonster_2 sm2_1 = Instantiate(spider_2Prefab, transform.position, Quaternion.identity);
                    sm2_1.name = $"Spider2_{monsterCount}";
                    sm2_1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 8:
                if(cycle == 0)      // 1사이클이면
                {
                    // 기본 쥐 몬스터, 기본 유령 몬스터 소환
                    spawnDelay = 0.5f;
                    MouseMonster_1 mm1_2 = Instantiate(mouse_1Prefab, transform.position, Quaternion.identity);
                    mm1_2.name = $"Mouse1_{monsterCount}";
                    mm1_2.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    GhostMonster_1 gm1_1 = Instantiate(ghost_1Prefab, transform.position, Quaternion.identity);
                    gm1_1.name = $"Ghost1_{monsterCount}";
                    gm1_1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 쥐 몬스터, 강화 유령 몬스터 소환
                    spawnDelay = 0.5f;
                    MouseMonster_2 mm2_2 = Instantiate(mouse_2Prefab, transform.position, Quaternion.identity);
                    mm2_2.name = $"Mouse2_{monsterCount}";
                    mm2_2.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    GhostMonster_2 gm2_1 = Instantiate(ghost_2Prefab, transform.position, Quaternion.identity);
                    gm2_1.name = $"Ghost2_{monsterCount}";
                    gm2_1.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 9:
                if(cycle == 0)          // 1사이클이면
                {
                    // 기본 거미 몬스터, 기본 유령 몬스터 소환
                    spawnDelay = 0.5f;
                    SpiderMonster_1 sm1_2 = Instantiate(spider_1Prefab, transform.position, Quaternion.identity);
                    sm1_2.name = $"Spider1_{monsterCount}";
                    sm1_2.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    GhostMonster_1 gm1_2 = Instantiate(ghost_1Prefab, transform.position, Quaternion.identity);
                    gm1_2.name = $"Ghost1_{monsterCount}";
                    gm1_2.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if(cycle == 1)     // 2사이클이면
                {
                    // 강화 거미 몬스터, 강화 유령 몬스터 소환
                    spawnDelay = 0.5f;
                    SpiderMonster_2 sm2_2 = Instantiate(spider_2Prefab, transform.position, Quaternion.identity);
                    sm2_2.name = $"Spider2_{monsterCount}";
                    sm2_2.transform.parent = monsterRepository.transform;
                    monsterCount++;

                    GhostMonster_2 gm2_2 = Instantiate(ghost_2Prefab, transform.position, Quaternion.identity);
                    gm2_2.name = $"Ghost2_{monsterCount}";
                    gm2_2.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
            case 10:
                if(cycle == 0)
                {
                    spawnDelay = 1.0f;
                    CyclopsMonster cm = Instantiate(cyclopsPrefab, transform.position, Quaternion.identity);
                    cm.name = $"Cyclops_{monsterCount}";
                    cm.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                else if (cycle == 1)
                {
                    spawnDelay = 1.0f;
                    LichMonster lm = Instantiate(lichPrefab, transform.position, Quaternion.identity);
                    lm.name = $"Lich_{monsterCount}";
                    lm.transform.parent = monsterRepository.transform;
                    monsterCount++;
                }
                break;
        }
    }

    /// <summary>
    /// 턴이 시작되었을 때 코루틴을 시작시키는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnTurnStartFC(int _)
    {
        Debug.Log("OnTurnStartFC 실행");
        StartCoroutine(SpawnerEnemyCoroutine(3.0f));
    }

    /// <summary>
    /// 몬스터를 스폰시키는 코루틴
    /// </summary>
    /// <param name="delay">턴 시작 후 몬스터가 스폰될 때까지 대기하는 시간</param>
    /// <returns></returns>
    IEnumerator SpawnerEnemyCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);             // delay 만큼 기다리고
        while (monsterCount < maxMonsterCount)              // 10번 실행
        {
            SpawnerEnemy();                                 // 몬스터 스폰
            yield return new WaitForSeconds(spawnDelay);    // delay 만큼 기다리고
        }
        monsterCount = 0;                                   // 몬스터를 전부 생성한 후 초기화
    }

#if UNITY_EDITOR
    public void Test_SpawnEnemy()
    {
        StartCoroutine(SpawnerEnemyCoroutine(3.0f));
    }
#endif
}
