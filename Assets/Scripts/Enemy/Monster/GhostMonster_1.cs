using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostMonster_1 : MonsterBase
{
    /// <summary>
    /// 안보이는 시간
    /// </summary>
    float invisibleTime = 1.5f;

    /// <summary>
    /// 이미지 비활성을 위한 렌더러
    /// </summary>
    Renderer ghostRenderer;

    /// <summary>
    /// 콜라이더를 끄기 위해 있음
    /// </summary>
    Collider2D ghostCollider;

    /// <summary>
    /// attackList에서 빼기 위해 있음
    /// </summary>
    AttackBase attackBase;

    protected override void Start()
    {
        moveSpeed = 3;
        waitTime = 0;
        hp = 50.0f;

        ghostRenderer = GetComponent<Renderer>();
        ghostCollider = GetComponent<Collider2D>();

        StartCoroutine(InvisibleCoroutine());
        
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))     // 충돌한 대상이 Enemy 태그가 아니면
        {
            attackBase = collision.GetComponent<AttackBase>();      // 충돌한 대상에서 attackBase를 가져옴
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            if(attackBase != null)
            {
                attackBase = null;
            }
        }
    }

    /// <summary>
    /// 이 게임 오브젝트를 보였다 안보였다 반복하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvisibleCoroutine()
    {
        while (hp > 1)      // hp 가 1보다 크면 발동
        {
            yield return new WaitForSeconds(invisibleTime);     // invisibleTime 만큼 기다리고

            ghostRenderer.enabled = false;                      // 렌더러 끄기
            ghostCollider.enabled = false;                      // 콜라이더 끄기
            if(attackBase != null)
            {
                if (attackBase.attackList.Contains(this))           // 리스트에 이 몬스터가 있었으면
                {
                    attackBase.attackList.Remove(this);             // 리스트에서 제거
                }
            }

            yield return new WaitForSeconds(invisibleTime);     // invisibleTime 만큼 기다리고

            ghostRenderer.enabled = true;                       // 렌더러 켜기
            ghostCollider.enabled = true;                       // 콜라이더 켜기

        }
    }
}
