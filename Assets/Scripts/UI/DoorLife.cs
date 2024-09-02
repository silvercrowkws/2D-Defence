using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorLife : MonoBehaviour
{
    GameManager gameManager;

    /// <summary>
    /// 문의 남은 체력을 확인 시키는 UI
    /// </summary>
    TextMeshProUGUI point;

    private void Awake()
    {
        Transform child = transform.GetChild(2);

        point = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onDoorLifeChange += OnDoorLifeChange;
    }

    /// <summary>
    /// 문의 남은 체력을 받아서 UI 를 바꾸는 함수
    /// </summary>
    /// <param name="doorLifePoint">문의 남은 체력</param>
    private void OnDoorLifeChange(int doorLifePoint)
    {
        point.text = doorLifePoint.ToString();
    }
}
