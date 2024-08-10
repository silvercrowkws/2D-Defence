using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    /// <summary>
    /// Yes 버튼
    /// </summary>
    Button yesButton;

    /// <summary>
    /// No 버튼
    /// </summary>
    Button noButton;

    /// <summary>
    /// 파괴 버튼
    /// </summary>
    Button destroyButton;

    /// <summary>
    /// Upgrade? 텍스트
    /// </summary>
    TextMeshProUGUI upgradeText;

    /*/// <summary>
    /// Yes 버튼의 이미지
    /// </summary>
    Image yesButtonImage;

    /// <summary>
    /// No 버튼의 이미지
    /// </summary>
    Image noButtonImage;

    /// <summary>
    /// Yes 버튼의 컬러
    /// </summary>
    Color yesColor;

    /// <summary>
    /// No 버튼의 컬러
    /// </summary>
    Color noColor;

    // 색상
    const float fadeColor = 0.0f;
    const float appearColor = 1.0f;*/

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    Vector3 screenPosition;

    /// <summary>
    /// 배치된 solider를 파괴시키라고 알리는 델리게이트
    /// </summary>
    public Action onDestroyButton;

    /// <summary>
    /// AttackBase에게 공격 범위의 알파를 0으로 바꾸라고 알리는 델리게이트
    /// </summary>
    public Action onAlphZero;    

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(YesUpgrade);
        //yesButton.gameObject.SetActive(false);              // 처음엔 게임 오브젝트 안보이게 만들기

        /*yesButtonImage = yesButton.GetComponent<Image>();
        yesColor = yesButtonImage.alphaColor;
        yesColor.a = fadeColor;                             // 알파값 0으로 만들기
        yesButtonImage.alphaColor = yesColor;
        yesButtonImage.raycastTarget = false;               // 처음엔 raycastTarget 끄기*/

        child = transform.GetChild(1);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(NoUpgrade);
        //noButton.gameObject.SetActive(false);               // 처음엔 게임 오브젝트 안보이게 만들기

        child = transform.GetChild(2);
        destroyButton = child.GetComponent<Button>();
        destroyButton.onClick.AddListener(DestroyFC);

        child = transform.GetChild(3);
        upgradeText = child.GetComponent <TextMeshProUGUI>();

        SetActiveFalse();

        /*noButtonImage = noButton.GetComponent<Image>();
        noColor = noButtonImage.alphaColor;
        noColor.a = fadeColor;                              // 알파값 0으로 만들기
        noButtonImage.alphaColor = noColor;
        noButtonImage.raycastTarget = false;                // 처음엔 raycastTarget 끄기*/

        player = GameManager.Instance.Player;
    }

    private void Start()
    {
        player.onClickedTileTransform += UpgradeButtonPosition;
    }

    /// <summary>
    /// 플레이어의 델리게이트를 받아서 강화 버튼의 위치를 바꾸는 함수
    /// </summary>
    /// <param name="cellPosition">solider가 클릭된 위치</param>
    private void UpgradeButtonPosition(Vector3Int cellPosition)
    {
        // 월드 좌표를 스크린 좌표로 변환
        screenPosition = Camera.main.WorldToScreenPoint(cellPosition);

        // 스크린 좌표를 RectTransform의 로컬 좌표로 변환
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            screenPosition,
            null,
            out localPoint
        );

        // UI 버튼의 위치를 설정
        rectTransform.anchoredPosition = localPoint;

        SetActiveTrue();

        /*yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);*/

        /*yesColor.a = appearColor;                   // 알파값 1로 만들어 보이게 만들기
        yesButtonImage.alphaColor = yesColor;

        noColor.a = appearColor;
        noButtonImage.alphaColor = noColor;

        yesButtonImage.raycastTarget = true;        // raycastTarget 켜기
        noButtonImage.raycastTarget = true;*/
    }

    /// <summary>
    /// Yes 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    private void YesUpgrade()
    {
        SetActiveFalse();
        player.boardClickAble = true;
    }

    /// <summary>
    /// No 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    private void NoUpgrade()
    {
        SetActiveFalse();
        player.boardClickAble = true;

        /*yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);*/

        /*yesColor.a = fadeColor;                     // 알파값 안보이게 만들기
        yesButtonImage.alphaColor = yesColor;

        noColor.a = fadeColor;
        noButtonImage.alphaColor = noColor;

        yesButtonImage.raycastTarget = false;       // raycastTarget 끄기
        noButtonImage.raycastTarget = false;*/
    }

    /// <summary>
    /// 설치된 solider를 파괴하는 함수
    /// </summary>
    private void DestroyFC()
    {
        SetActiveFalse();
        player.boardClickAble = true;
        onDestroyButton?.Invoke();
    }

    /// <summary>
    /// 버튼들을 보이게 만드는 함수
    /// </summary>
    void SetActiveTrue()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        destroyButton.gameObject.SetActive(true);
        upgradeText.gameObject.SetActive(true);
    }

    /// <summary>
    /// 버튼들을 안보이게 만드는 함수
    /// </summary>
    public void SetActiveFalse()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        destroyButton.gameObject.SetActive(false);
        upgradeText.gameObject.SetActive(false);
        onAlphZero?.Invoke();
    }
}
