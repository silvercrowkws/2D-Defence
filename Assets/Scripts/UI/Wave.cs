using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    TextMeshProUGUI waveNumber;

    TurnManager turnManager;

    private void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();

        Transform child = transform.GetChild(0);
        waveNumber = child.GetComponent<TextMeshProUGUI>();

        turnManager.onTurnStart += WaveNumberChange;
    }

    /// <summary>
    /// 턴 매니저의 턴 시작 델리게이트를 받아 현재 몇 웨이브인지 UI를 변경하는 함수
    /// </summary>
    /// <param name="turnNumber"></param>
    private void WaveNumberChange(int turnNumber)
    {
        waveNumber.text = turnNumber.ToString();
    }
}
