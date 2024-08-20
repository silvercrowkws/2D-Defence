using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
    /// <summary>
    /// 0번째 자식 100의 자리
    /// </summary>
    Image image_100;

    /// <summary>
    /// 1번째 자식 10의 자리
    /// </summary>
    Image image_10;

    /// <summary>
    /// 2번째 자식 1의 자리
    /// </summary>
    Image image_1;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 숫자 스프라이트 배열
    /// </summary>
    public Sprite[] numbers;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        Transform child = transform.GetChild(0);
        image_100 = child.GetComponent<Image>();        // 0번째 자식 100의 자리

        child = transform.GetChild(1);
        image_10 = child.GetComponent<Image>();         // 1번째 자식 10의 자리

        child = transform.GetChild(2);
        image_1 = child.GetComponent<Image>();          // 2번째 자식 1의 자리

        gameManager.moneyChange += NumberImageChange;
    }

    /// <summary>
    /// 게임매니저의 델리게이트를 받아 UI를 변경하는 함수
    /// </summary>
    /// <param name="money">델리게이트로 받은 돈</param>
    private void NumberImageChange(float money)
    {
        // money를 정수로 변환
        int coin = Mathf.FloorToInt(money);

        // 100의 자리
        int coin_100 = (coin / 100) % 10;
        image_100.sprite = numbers[coin_100];

        // 10의 자리
        int coin_10 = (coin / 10) % 10;
        image_10.sprite = numbers[coin_10];

        // 1의 자리
        int coin_1 = coin % 10;
        image_1.sprite = numbers[coin_1];
    }
}
