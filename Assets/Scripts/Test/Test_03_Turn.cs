using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Turn : TestBase
{
#if UNITY_EDITOR
    TurnManager turnManager;
    EnemySpawner enemySpawner;

    private void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Debug.Log("첫 턴 시작");
        turnManager.OnTurnStart2();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log("턴 종료 시킴");
        turnManager.OnTurnEnd2();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        enemySpawner.Test_SpawnEnemy();
    }
#endif
}
