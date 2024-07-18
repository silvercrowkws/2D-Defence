using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsMonster : MonsterBase
{
    protected override void Start()
    {
        moveSpeed = 5;
        waitTime = 0;
        base.Start();
    }
}
