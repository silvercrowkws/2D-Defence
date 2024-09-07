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

    /// <summary>
    /// 돈이 변경되었음을 알리는 델리게이트(UI 수정용)
    /// </summary>
    public Action<float> moneyChange;

    /// <summary>
    /// 플레이어
    /// </summary>
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

    /// <summary>
    /// 현재 가지고 있는 돈
    /// </summary>
    float currentMoney;

    /// <summary>
    /// 돈 프로퍼티
    /// </summary>
    public float Money
    {
        get => currentMoney;
        set
        {
            if(currentMoney != value)
            {
                //currentMoney = value;
                currentMoney = Mathf.Clamp(value, 0, 999);
                Debug.Log($"남은 돈 : {currentMoney}");
                moneyChange?.Invoke(currentMoney);
            }
        }
    }

    /// <summary>
    /// 문의 남은 체력
    /// </summary>
    int doorLife;

    public int DoorLife
    {
        get => doorLife;
        set
        {
            if(doorLife != value)
            {
                doorLife = Mathf.Clamp(value, 0, 10);
                Debug.Log($"문의 남은 체력 : {doorLife}");
                onDoorLifeChange?.Invoke(doorLife);
            }
        }
    }

    /// <summary>
    /// 문의 체력이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<int> onDoorLifeChange;

    /*protected override void Awake()
    {
        base.Awake();
    }*/

    private void Start()
    {
        //gameState = GameState.GameReady;      // 위에서 바로 시작 시 게임 준비 상태로 됨
        currentMoney = 100.0f;       // 처음 시작 소지금
        //moneyChange?.Invoke(currentMoney);        // 게임 매니저의 Start가 UI 변경시키는 CoinText 클래스보다 빨리 실행되서 해도 의미가 없음

        //player.onCellPosition += SoliderSet;

        doorLife = 10;              // 문의 체력
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();

        turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.OnInitialize2();
    }

    /// <summary>
    /// 게임을 재시작 시킬때 초기화 시키는 함수
    /// </summary>
    public void GameRestart()
    {
        Time.timeScale = 1.0f;                  // 타임 스케일 조정
        gameState = GameState.GameReady;        // 게임 준비 상태로 전환
        player.soliderObjectDictionary.Clear(); // 설치된 솔저 오브젝트 Dictionary(키 : 위치, 값 : 오브젝트) 초기화
        player.objectSoliderDictionary.Clear(); // 설치된 오브젝트 솔저 Dictionary(키 : 오브젝트, 값 : 위치) 초기화
        currentMoney = 100.0f;                   // 소지금 초기화
        doorLife = 10;                          // 문의 체력 초기화
        turnManager.turnNumber = 0;
        //turnManager.OnInitialize2();            // 씬이 시작될 때 
    }





#if UNITY_EDITOR


#endif
}
