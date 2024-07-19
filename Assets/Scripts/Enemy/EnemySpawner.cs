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
    int monsterCount;

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

    private void Awake()
    {
        // 월드 좌표계의 transform.position을 타일맵의 셀 좌표계의 Vector3Int로 변환
        /*spawnerPosition = enemyTilemap.WorldToCell(transform.position);

        List<GameObject> waypointList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
            {
                waypointList.Add(child.gameObject);
            }
        }
        waypoints = waypointList.ToArray();
        Debug.Log($"Found {waypoints.Length} waypoints.");*/
    }

    private void Start()
    {
        monsterRepository = GameObject.Find("MonsterRepository");
        turnManager = FindAnyObjectByType<TurnManager>();
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

        /*if (turnManager.turnNumber == 1)
        {
            Debug.Log("1턴 쥐 몬스터 소환");
        }
        else if(turnManager.turnNumber == 2)
        {
            Debug.Log("2턴 쥐 몬스터 소환");
        }
        else if(turnManager.turnNumber == 3)
        {
            Debug.Log("3턴 거미 몬스터 소환");
        }
        else if(turnManager.turnNumber == 4)
        {
            Debug.Log("4턴 거미 몬스터 소환");
        }
        else if(turnManager.turnNumber == 5)
        {
            Debug.Log("5턴 유령 몬스터 소환");
        }
        else if(turnManager.turnNumber == 6)
        {
            Debug.Log("6턴 유령 몬스터 소환");
        }
        else if( turnManager.turnNumber == 7)
        {
            Debug.Log("7턴 쥐 반, 거미 반 소환");
        }
        else if(turnManager.turnNumber== 8)
        {
            Debug.Log("8턴 쥐 반, 유령 반 소환");
        }
        else if (turnManager.turnNumber== 9)
        {
            Debug.Log("9턴 거미 반, 유령 반 소환");
        }
        else if(turnManager.turnNumber == 10)
        {
            Debug.Log("10턴 사이클롭스 몬스터 소환");
        }
        if (turnManager.turnNumber == 11)
        {
            Debug.Log("11턴 강화 쥐 몬스터 소환");
        }
        else if (turnManager.turnNumber == 12)
        {
            Debug.Log("12턴 강화 쥐 몬스터 소환");
        }
        else if (turnManager.turnNumber == 13)
        {
            Debug.Log("13턴 강화 거미 몬스터 소환");
        }
        else if (turnManager.turnNumber == 14)
        {
            Debug.Log("14턴 강화 거미 몬스터 소환");
        }
        else if (turnManager.turnNumber == 15)
        {
            Debug.Log("15턴 강화 유령 몬스터 소환");
        }
        else if (turnManager.turnNumber == 16)
        {
            Debug.Log("16턴 강화 유령 몬스터 소환");
        }
        else if (turnManager.turnNumber == 17)
        {
            Debug.Log("17턴 강화 쥐 반, 강화 거미 반 소환");
        }
        else if (turnManager.turnNumber == 18)
        {
            Debug.Log("18턴 강화 쥐 반, 강화 유령 반 소환");
        }
        else if (turnManager.turnNumber == 19)
        {
            Debug.Log("19턴 강화 거미 반, 강화 유령 반 소환");
        }
        else if(turnManager.turnNumber == 20)
        {
            Debug.Log("20턴 리치 몬스터 소환");
        }*/
    }

    /// <summary>
    /// 몬스터를 스폰시키는 코루틴
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator SpawnerEnemyCoroutine(float delay)
    {
        //yield return new WaitForSeconds(1.0f);          // 1초 기다리고(몬스터마다 달라야 하는데?)
        while (monsterCount < 10)                       // 10번 실행
        {
            SpawnerEnemy();                             // 몬스터 스폰
            yield return new WaitForSeconds(spawnDelay);     // delay 만큼 기다리고
        }
        monsterCount = 0;
    }

#if UNITY_EDITOR
    public void Test_SpawnEnemy()
    {
        StartCoroutine(SpawnerEnemyCoroutine(3.0f));
    }
#endif
}
