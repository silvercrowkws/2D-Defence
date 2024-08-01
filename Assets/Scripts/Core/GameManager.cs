using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임상태
/// </summary>
public enum GameState
{
    GameReady,
    GameStart,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 현재 게임상태
    /// </summary>
    GameState gameState = GameState.GameReady;

    /// <summary>
    /// 현재 게임상태 변경시 알리는 프로퍼티
    /// </summary>
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                switch (gameState)
                {
                    case GameState.GameReady:
                        Debug.Log("게임레디");
                        onGameReady?.Invoke();
                        break;
                    case GameState.GameStart:
                        Debug.Log("게임스타트");
                        onGameStart?.Invoke();
                        break;
                    case GameState.GameOver:
                        Debug.Log("게임오버");
                        onGameOver?.Invoke();
                        break;
                }
            }
        }
    }


    // 게임상태 델리게이트
    public Action onGameReady;
    public Action onGameStart;
    public Action onGameOver;

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

    /*protected override void Awake()
    {
        base.Awake();
    }*/

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
