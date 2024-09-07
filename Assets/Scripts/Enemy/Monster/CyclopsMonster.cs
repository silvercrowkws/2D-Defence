using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsMonster : MonsterBase
{
    Player player;
    GameManager gameManager;

    /// <summary>
    /// 유닛을 파괴할 때 보일 레이저 루트
    /// </summary>
    GameObject laserRoot;

    protected override void Start()
    {
        moveSpeed = 2.5f;
        waitTime = 0.5f;
        currentHp = 700.0f;
        maxHP = currentHp;

        gameManager = GameManager.Instance;
        player = GameManager.Instance.Player;

        Transform child = transform.GetChild(0);
        laserRoot = child.gameObject;
        laserRoot.SetActive(false);

        dieMoney = 15.0f;

        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 바바리안 or 워리어 or 위자드 일 경우
        if (collision.CompareTag("Barbarian") || collision.CompareTag("Warrior") || collision.CompareTag("Wizard") || collision.CompareTag("Barbarian2") || collision.CompareTag("Warrior2") || collision.CompareTag("Wizard2"))
        {
            if (Random.value < 0.05f)        // 10% 의 확률로 => 5% 로 조정
            {
                GameObject collisionGameObject = collision.gameObject;          // 충돌한 게임 오브젝트
                player.objectSoliderDictionary.TryGetValue(collisionGameObject, out Vector3Int DestroyCellPosition);

                player.soliderTilemap.SetTile(DestroyCellPosition, null);       // 타일맵에서 파괴
                Destroy(collisionGameObject);                                   // 오브젝트 파괴
                player.soliderObjectDictionary.Remove(DestroyCellPosition);     // Dictionary에서 제거
                player.objectSoliderDictionary.Remove(collisionGameObject);     // Dictionary에서 제거

                StartCoroutine(LaserCoroutine(collisionGameObject));

                Debug.Log("10% 유닛 파괴");
            }
        }
    }

    /// <summary>
    /// 충돌시 10% 확률로 유닛을 제거하는 코루틴
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator LaserCoroutine(GameObject target)
    {
        laserRoot.transform.position = this.gameObject.transform.position;      // 위치 맞추고
        laserRoot.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);    // 각도 초기화 하고
        laserRoot.gameObject.SetActive(true);                                   // 게임 오브젝트 활성화

        Transform laserTransform = laserRoot.gameObject.transform;              // laserRoot의 Transform 가져오기

        // 타겟과 laserRoot 사이의 방향 벡터 계산
        Vector3 direction = target.transform.position - laserTransform.position;

        // 방향 벡터의 각도를 계산하여 laserRoot의 rotation을 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));      // Atan2로 얻은 각도는 유니티의 기본 방향과 맞지 않아
                                                                                        // 90도를 더해줌
        yield return new WaitForSeconds(0.2f);                                          // 0.2초 후
        laserRoot.gameObject.SetActive(false);                                          // 게임 오브젝트 비활성화
    }


}
