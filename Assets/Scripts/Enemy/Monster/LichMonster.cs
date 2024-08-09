using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichMonster : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 1.0f;
        waitTime = 0.5f;
        hp = 300.0f;
        base.Start();
    }
    // 리치는 특수 능력 뭔가 하나 넣자
}
