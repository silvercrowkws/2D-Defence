using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMonster_2 : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 5;
        waitTime = 0;
        hp = 45.0f;
        base.Start();
    }
}
