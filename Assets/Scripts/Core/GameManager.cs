using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //player.onCellPosition += SoliderSet;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();

        turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.OnInitialize2();
    }

    /*/// <summary>
    /// 솔저를 배치하는 함수
    /// </summary>
    private void SoliderSet(Vector3Int soliderPosition)
    {

    }

    /// <summary>
    /// 솔저를 강화하는 함수
    /// </summary>
    private void SoliderUpgrade()
    {

    }*/






#if UNITY_EDITOR


#endif
}
