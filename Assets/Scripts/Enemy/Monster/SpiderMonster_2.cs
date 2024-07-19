using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMonster_2 : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 4;
        waitTime = 0;
        hp = 45.0f;
        base.Start();
    }
}
