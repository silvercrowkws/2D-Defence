using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsMonster : MonsterBase
{
    Player player;
    GameManager gameManager;

    protected override void Start()
    {
        moveSpeed = 2.5f;
        waitTime = 0.5f;
        hp = 100.0f;

        gameManager = GameManager.Instance;
        player = GameManager.Instance.Player;

        base.Start();

        /*if (Random.value < 0.1f)
        {
            Debug.Log("10% 확률로 출력되는 메시지");
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 바바리안 or 워리어 or 위자드 일 경우
        if (collision.CompareTag("Barbarian") || collision.CompareTag("Warrior") || collision.CompareTag("Wizard"))
        {
            if (Random.value < 0.1f)        // 10% 의 확률로
            {
                GameObject collisionGameObject = collision.gameObject;          // 충돌한 게임 오브젝트
                player.objectSoliderDictionary.TryGetValue(collisionGameObject, out Vector3Int DestroyCellPosition);

                player.soliderTilemap.SetTile(DestroyCellPosition, null);       // 타일맵에서 파괴
                Destroy(collisionGameObject);                                   // 오브젝트 파괴
                player.soliderObjectDictionary.Remove(DestroyCellPosition);     // Dictionary에서 제거
                player.objectSoliderDictionary.Remove(collisionGameObject);     // Dictionary에서 제거

                Debug.Log("10% 유닛 파괴");
            }
        }
    }


}
