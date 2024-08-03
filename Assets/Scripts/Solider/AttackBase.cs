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
    protected float attackSpeed = 1.0f;

    //MonsterBase monsterBase;

    /// <summary>
    /// 누적 시간
    /// </summary>
    float elTime;

    public GameObject Lightning;

    Factory factory;

    private void Awake()
    {
        factory = Factory.Instance;
    }

    protected virtual void Start()
    {
        attackList = new List<MonsterBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("적과 충돌");
            MonsterBase monsterBase = collision.gameObject.GetComponent<MonsterBase>();     // 충돌한 게임 오브젝트에서 MonsterBase를 가져옴
            
            if (monsterBase != null && !attackList.Contains(monsterBase))       // 몬스터가 있고, 그 몬스터가 리스트에 포함되어 있지 않으면(중복된 몬스터가 리스트에 추가되는 것 방지)
            {
                monsterBase.onDie = () =>
                {
                    attackList.Remove(monsterBase);     // 죽으면 리스트에서 삭제
                };
                attackList.Add(monsterBase);
                //attackList.Insert(0, monsterBase);      // 충돌한 적을 리스트의 0번으로
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
            monster.onDie = () =>
            {
                attackList.Remove(monster);
            };

            if (elTime > attackSpeed)       // 누적 시간이 공격 속도보다 크면
            {
                AttackProcess();            // 공격 함수 실행
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterBase monster = collision.gameObject.GetComponent<MonsterBase>();
            if (monster != null && attackList.Contains(monster))        // 몬스터가 있고, 그 몬스터가 리스트에 포함되어 있으면
            {
                attackList.Remove(monster);     // 리스트에서 제거
            }
        }
    }

    private void Update()
    {
        if(attackList.Count > 0)        // attackList에 뭔가 있으면
        {
            elTime += Time.deltaTime;   // 시간 누적
        }
        else                            // attackList에 뭔가 없으면
        {
            elTime = 0;                 // 시간 초기화
        }
    }

    /// <summary>
    /// 공격 함수
    /// </summary>
    protected virtual void AttackProcess()
    {
        elTime = 0;                 // 공격 함수 발동 시 누적 시간 초기화

        if(attackList[0].HP > 0)    // attackList의 0번 HP가 0보다 크면
        {
            if (transform.CompareTag("Wizard"))             // 마법사의 경우
            {
                // 만약 본인이 마법사라면 리스트에 있는 모든 몬스터 공격
                for (int i = 0; i < attackList.Count; i++)      // 공격하는 순간에도 리스트의 변형이 일어나기 때문에 foreach문 안됨
                {
                    //Instantiate(Lightning, attackList[i].transform.position, Quaternion.identity);
                    factory.GetLightning(attackList[i].transform.position);   // 팩토리를 이용해 번개 생성
                    //Debug.Log($"{i}번째 리스트의 위치 : {attackList[i].transform.position}");

                    attackList[i].HP -= damage;
                }
            }
            else if(transform.CompareTag("Warrior"))        // 전사의 경우
            {
                factory.GetWeapon_Warrior_01(attackList[0].transform.position, 180.0f);     // 팩토리를 이용해 검 생성
                //Debug.Log($"0번째 리스트의 위치 : {attackList[0].transform.position}");

                attackList[0].HP -= damage;     // damage 만큼 HP 깍음
            }
            else if (transform.CompareTag("Barbarian"))     // 바바리안의 경우
            {
                factory.GetWeapon_Barbarian_01(attackList[0].transform.position, 180.0f);   // 팩토리를 이용해 도끼 생성
                //Debug.Log($"0번째 리스트의 위치 : {attackList[0].transform.position}");

                attackList[0].HP -= damage;
            }
            //Debug.Log($"{attackList[0].name} 공격");        // HP 가 0이되는 순간 onDie가 발동되서 리스트에서 제거되기 때문에 오류 있음
            //Debug.Log($"{attackList[0].name}의 남은 HP : {attackList[0].HP}");
        }
    }

    /*/// <summary>
    /// 공격 반복용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine()
    {
        while (attackList.Count > 0)
        {
            if (attackList[0].HP > 0)
            {
                attackList[0].HP -= damage;                         // 공격
                Debug.Log($"{attackList[0].name} 공격");
                Debug.Log($"{attackList[0].name}의 남은 HP : {attackList[0].HP}");
            }

            if (attackList[0].HP < 1)
            {
                attackList.RemoveAt(0);
            }
            yield return new WaitForSeconds(attackSpeed);       // 공격 쿨타임 만큼 기다리기
        }
    }*/
}
