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
    public float elTime;

    //public GameObject Lightning;

    /// <summary>
    /// 팩토리
    /// </summary>
    Factory factory;

    /// <summary>
    /// 공격 못할 때 보여질 게임 오브젝트
    /// </summary>
    public GameObject spiderWeb;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 스프라이트
    /// </summary>
    SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// 알파값을 조절하기 위한 컬러
    /// </summary>
    Color alphaColor;

    Upgrade upgrade;

    private void Awake()
    {
        factory = Factory.Instance;
        spiderWeb = transform.GetChild(0).gameObject;
        spiderWeb.SetActive(false);       // 게임 시작시에는 안보이게
        
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        alphaColor = spriteRenderer.color;
        alphaColor.a = 0;
        spriteRenderer.color = alphaColor;      // 처음엔 안보이게
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;

        attackList = new List<MonsterBase>();

        player.onSoliderCilck += onAlphaChange;
        player.onNonSoliderClick += onAlphaChange2;

        upgrade = FindAnyObjectByType<Upgrade>();

        if (upgrade != null)
        {
            upgrade.onAlphZero += onAlphaChange2;
        }
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
            MonsterBase monsterBase = collision.gameObject.GetComponent<MonsterBase>();
            monsterBase.onDie = () =>
            {
                attackList.Remove(monsterBase);
            };

            if (elTime > attackSpeed && attackList.Count > 0)       // 누적 시간이 공격 속도보다 크면
            {
                AttackProcess();            // 공격 함수 실행
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterBase monsterBase = collision.gameObject.GetComponent<MonsterBase>();
            if (monsterBase != null && attackList.Contains(monsterBase))        // 몬스터가 있고, 그 몬스터가 리스트에 포함되어 있으면
            {
                attackList.Remove(monsterBase);     // 리스트에서 제거
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

        if(attackList.Count > 0 && attackList[0].HP > 0)    // attackList의 0번 HP가 0보다 크면
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

    /// <summary>
    /// 상대가 stopTime초 동안 공격 못하게 하는 코루틴
    /// </summary>
    /// <param name="stopTime"></param>
    /// <returns></returns>
    public IEnumerator AttackStop(float stopTime)
    {
        yield return new WaitForSeconds(stopTime);      // stopTime 만큼 기다리고
        spiderWeb.SetActive(false);                     // 거미줄 비활성화
        elTime = 0;                                     // 누적 시간 정상화
        Debug.Log("공격 재개");
    }

    /// <summary>
    /// 다시 공격을 시작하는 코루틴
    /// </summary>
    public void OnAttackStart(float stopTime)
    {
        StartCoroutine(AttackStop(stopTime));
    }

    /// <summary>
    /// 스프라이트 렌더러의 알파를 조절하는 함수
    /// </summary>
    private void onAlphaChange()
    {
        if (spriteRenderer != null)
        {
            alphaColor.a = 0.1f;
            spriteRenderer.color = alphaColor;
        }
    }

    /// <summary>
    /// 스트라이트 렌더러의 알파를 안보이게 조절하는 함수
    /// </summary>
    private void onAlphaChange2()
    {
        //if (this.gameObject != null)
        if (spriteRenderer != null)
        {
            alphaColor.a = 0;
            spriteRenderer.color = alphaColor;
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
