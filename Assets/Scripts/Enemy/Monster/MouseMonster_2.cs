using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMonster_2 : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 5;
        waitTime = 0;
        currentHp = 75.0f;
        maxHP = currentHp;
        dieMoney = 10.0f;
        base.Start();
    }
}
