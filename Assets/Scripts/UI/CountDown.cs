using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    GameManager gameManager;

    /// <summary>
    /// 카운트 다운 텍스트
    /// </summary>
    TextMeshProUGUI countDownText;

    /// <summary>
    /// 카운트 다운을 시작해도 되는지 확인하는 bool 변수
    /// </summary>
    bool countDownStart = false;

    /// <summary>
    /// 실제로 보여질 시간
    /// </summary>
    float countDownTime = 10;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        turnManager = FindAnyObjectByType<TurnManager>();
        //turnManager.onInitialize2Start += OnCountDown;
        turnManager.onInitialize2Start += () =>
        {
            countDownStart = true;
        };

        countDownText = GetComponent<TextMeshProUGUI>();
    }


    private void Update()
    {
        if(countDownStart && countDownTime > 0)                 // 만약 카운트 다운을 시작해도 되고, 카운트 다운 시간이 0보다 크면
        {
            countDownTime -= Time.deltaTime;                    // countDownTime에서 초당 빼고
            countDownText.text = $"{countDownTime:f1}";         // UI 갱신(소수점 1자리 까지)
        }
        else                                                    // 아니면
        {
            gameManager.GameState = GameState.GameStart;        // 게임 상태를 Start로 변경
            turnManager.OnTurnStart2();                         // 턴 시작시키고
            this.gameObject.SetActive(false);                   // UI 안보이게 만들기
        }
    }
}
