using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Start()
    {
        moveSpeed = 1.0f;
        waitTime = 0.5f;
        hp = 300.0f;
        healingAmount = (hp / 3);
        healingInterval = 2.0f;
        StartCoroutine(HealingCoroutine());
        base.Start();
    }
    
    /// <summary>
    /// 회복 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator HealingCoroutine()
    {
        yield return null;
        while(HP > 1)       // HP가 1보다 크면 반복 => HP를 초과해서도 회복 가능
        {
            yield return new WaitForSeconds(healingInterval);
            //Debug.Log($"회복 전 체력 : {HP}");
            HP += healingAmount;
            //Debug.Log($"회복 후 체력 : {HP}");
        }
    }
}
