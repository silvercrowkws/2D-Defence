using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMonster_1 : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 5;
        waitTime = 0;
        hp = 50.0f;
        base.Start();
    }
}
