using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMonster_2 : MonsterBase
{
    Player player;
    GameManager gameManager;

    /// <summary>
    /// attackList에서 빼기 위해 있음
    /// </summary>
    AttackBase attackBase;

    /// <summary>
    /// 공격을 멈추게 하는 시간
    /// </summary>
    float stopTime = 1.0f;

    protected override void Start()
    {
        moveSpeed = 4;
        waitTime = 0;
        currentHp = 150.0f;
        maxHP = currentHp;
        dieMoney = 10.0f;
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 바바리안 or 워리어 or 위자드 일 경우
        if (collision.CompareTag("Barbarian") || collision.CompareTag("Warrior") || collision.CompareTag("Wizard"))
        {
            if (UnityEngine.Random.value < 0.1f)        // 10% 의 확률로
            {
                attackBase = collision.GetComponent<AttackBase>();      // 충돌한 대상에서 attackBase를 가져옴
                attackBase.elTime = -100;       // 공격 못하도록 -100(elTime이 attackSpeed 보다 크면 공격하는 시스템)
                attackBase.spiderWeb.SetActive(true);
                Debug.Log("공격 정지");
                attackBase.OnAttackStart(stopTime);
                //StartCoroutine(attackBase.AttackStop(stopTime));
            }
        }
    }
}
