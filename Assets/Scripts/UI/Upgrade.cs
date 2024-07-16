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
    /// Upgrade? 텍스트
    /// </summary>
    TextMeshProUGUI upgradeText;

    /// <summary>
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
    const float appearColor = 1.0f;


    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(YesUpgrade);
        //yesButton.gameObject.SetActive(false);              // 처음엔 게임 오브젝트 안보이게 만들기

        /*yesButtonImage = yesButton.GetComponent<Image>();
        yesColor = yesButtonImage.color;
        yesColor.a = fadeColor;                             // 알파값 0으로 만들기
        yesButtonImage.color = yesColor;
        yesButtonImage.raycastTarget = false;               // 처음엔 raycastTarget 끄기*/

        child = transform.GetChild(1);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(NoUpgrade);
        //noButton.gameObject.SetActive(false);               // 처음엔 게임 오브젝트 안보이게 만들기

        child = transform.GetChild(2);
        upgradeText = child.GetComponent <TextMeshProUGUI>();

        SetActiveFalse();

        /*noButtonImage = noButton.GetComponent<Image>();
        noColor = noButtonImage.color;
        noColor.a = fadeColor;                              // 알파값 0으로 만들기
        noButtonImage.color = noColor;
        noButtonImage.raycastTarget = false;                // 처음엔 raycastTarget 끄기*/
    }

    private void Start()
    {
        player = GameManager.Instance.Player;

        player.onClickedTileTransform += UpgradeButtonPosition;
    }

    /// <summary>
    /// 플레이어의 델리게이트를 받아서 강화 버튼의 위치를 바꾸는 함수
    /// </summary>
    /// <param name="cellPosition">solider가 클릭된 위치</param>
    private void UpgradeButtonPosition(Vector3Int cellPosition)
    {
        // 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(cellPosition);

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
        yesButtonImage.color = yesColor;

        noColor.a = appearColor;
        noButtonImage.color = noColor;

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
        yesButtonImage.color = yesColor;

        noColor.a = fadeColor;
        noButtonImage.color = noColor;

        yesButtonImage.raycastTarget = false;       // raycastTarget 끄기
        noButtonImage.raycastTarget = false;*/
    }

    /// <summary>
    /// 버튼들을 보이게 만드는 함수
    /// </summary>
    void SetActiveTrue()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        upgradeText.gameObject.SetActive(true);
    }

    /// <summary>
    /// 버튼들을 안보이게 만드는 함수
    /// </summary>
    public void SetActiveFalse()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        upgradeText.gameObject.SetActive(false);
    }
}
