using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LichMonster : MonsterBase
{
    /// <summary>
    /// 회복량(HP의 1/3)
    /// </summary>
    float healingAmount;

    /// <summary>
    /// 회복 간격
    /// </summary>
    float healingInterval;

    /// <summary>
    /// 파티클 시스템
    /// </summary>
    ParticleSystem particle;

    protected override void Start()
    {
        moveSpeed = 1.0f;
        waitTime = 0.5f;
        hp = 300.0f;
        healingAmount = (hp / 3);
        healingInterval = 2.0f;

        Transform child = transform.GetChild(0);
        particle = child.GetComponent<ParticleSystem>();
        particle.Stop();

        StartCoroutine(HealingCoroutine());

        base.Start();
    }
    
    /// <summary>
    /// 회복 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator HealingCoroutine()
    {
        while (HP > 1)       // HP가 1보다 크면 반복 => HP를 초과해서도 회복 가능
        {
            /*yield return new WaitForSeconds(healingInterval);       // healingInterval만큼 기다리고
            particle.Play();                                        // 파티클 시작
            //Debug.Log($"회복 전 체력 : {HP}");
            HP += healingAmount;
            //Debug.Log($"회복 후 체력 : {HP}");*/

            particle.Play();        // 파티클 시작
            HP += healingAmount;    // 회복
            yield return new WaitForSeconds(healingInterval);       // healingInterval만큼 기다리고
            particle.Stop();        // 파티클 종료
        }
    }
}
