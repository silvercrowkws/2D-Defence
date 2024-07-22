using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    /// <summary>
    /// 공격 순서 리스트
    /// </summary>
    public List<MonsterBase> attackList;

    /// <summary>
    /// 공격력
    /// </summary>
    protected float damage = 10;

    /// <summary>
    /// 공격 쿨타임(빈도)
    /// </summary>
    protected float attackSpeed = 1;

    MonsterBase monster;

    protected virtual void Start()
    {
        attackList = new List<MonsterBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            monster = collision.gameObject.GetComponent<MonsterBase>();     // 충돌한 게임 오브젝트에서 MonsterBase를 가져옴

            if (monster != null)
            {
                attackList.Add(monster);        // 리스트에 누적

                //AttackProcess();                // 여기다가 시키면 충돌할 때마다 시켜서 안되는데..."충돌한게 있을 때" 로 해야할 듯
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AttackCheck();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();     // 충돌한 게임 오브젝트에서 MonsterBase를 가져옴

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (monster != null)
            {
                attackList.Remove(monster);     // 리스트에서 제거
            }
        }
    }

    void AttackCheck()
    {
        if(attackList.Count > 0)
        {
            AttackProcess();
        }
    }

    /// <summary>
    /// 공격 함수
    /// </summary>
    protected virtual void AttackProcess()
    {
        StartCoroutine(AttackCoroutine());
    }

    /// <summary>
    /// 공격 반복용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine()
    {
        while (attackList[0].HP > 0)
        {
            attackList[0].HP -= damage;                         // 공격
            Debug.Log($"{attackList[0].name} 공격");
            yield return new WaitForSeconds(attackSpeed);       // 공격 쿨타임 만큼 기다리기
        }
    }
}
