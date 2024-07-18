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
    /// cyclopsPrefab
    /// </summary>
    public CyclopsMonster cyclopsPrefab;

    GameObject monsterRepository;

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
    }

    /// <summary>
    /// 몬스터를 스폰시키는 함수
    /// </summary>
    void SpawnerEnemy()
    {
        monsterCount++;
        //enemyTilemap.SetTile(spawnerPosition, cyclopsTiles);
        Debug.Log($"{monsterCount} 마리째 몬스터 스폰");

        // 이 스크립트가 부착된 게임 오브젝트의 위치에서 프리팹을 생성
        //Instantiate(cyclopsPrefab, transform.position, Quaternion.identity);

        CyclopsMonster cm = Instantiate(cyclopsPrefab, transform.position, Quaternion.identity);
        cm.name = $"Cyclops_{monsterCount}";
        cm.transform.parent = monsterRepository.transform;

        // 몬스터 게임 오브젝트를 생성하고 Monster_Test 스크립트를 추가합니다.
        /*GameObject monster = new GameObject("Cyclops");
        monster.AddComponent<SpriteRenderer>().sprite = monsterSprite;              // 스프라이트 설정 (적절한 스프라이트로 교체 필요)
        monster.transform.position = enemyTilemap.CellToWorld(spawnerPosition);
        Monster_Test monsterScript = monster.AddComponent<Monster_Test>();
        monsterScript.waypoints = waypoints;*/
    }

    /// <summary>
    /// 몬스터를 스폰시키는 코루틴
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator SpawnerEnemyCoroutine(float delay)
    {
        yield return new WaitForSeconds(1.0f);          // 1초 기다리고(몬스터마다 달라야 하는데?)
        while (monsterCount < 10)                       // 10번 실행
        {
            SpawnerEnemy();                             // 몬스터 스폰
            yield return new WaitForSeconds(delay);     // delay 만큼 기다리고
        }
    }

#if UNITY_EDITOR
    public void Test_SpawnEnemy()
    {
        StartCoroutine(SpawnerEnemyCoroutine(3.0f));
    }
#endif
}
