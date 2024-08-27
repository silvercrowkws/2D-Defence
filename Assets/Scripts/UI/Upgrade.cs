using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    /// <summary>
    /// descriptionImage 이미지(설명용)
    /// </summary>
    Image descriptionImage;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    Vector3 screenPosition;

    /// <summary>
    /// 배치된 solider를 파괴시키라고 알리는 델리게이트
    /// </summary>
    public Action<bool> onDestroyButton;

    /// <summary>
    /// AttackBase에게 공격 범위의 알파를 0으로 바꾸라고 알리는 델리게이트
    /// </summary>
    public Action onAlphZero;

    /// <summary>
    /// Player에게 업그레이드 Yes 버튼이 눌렸다고 알리는 델리게이트
    /// </summary>
    public Action onUpgradeYesButton;

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
        upgradeText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(4);
        descriptionImage = child.GetComponent<Image>();

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


        Vector3 cornerPosition = screenPosition;

        // 화면의 너비를 기준으로 18분할 크기를 계산
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float sectionWidth = screenWidth / 18.0f;           // 가로 18 분할
        float sectionHeight = screenHeight / 10.0f;         // 세로 10 분할

        // 이동할 오프셋 값 (중앙으로 이동하기 위한 값)
        float xOffset = sectionWidth / 18.0f;
        float yOffset = sectionHeight / 10.0f;

        Vector3 defaltUpgrade = new Vector3(150, 260, 0);       // 기본 위치
        Vector3 changeUpgrade = new Vector3(150, -160, 0);      // 변경된 위치

        upgradeText.rectTransform.localPosition = defaltUpgrade;        // 함수 불러질때 위치 초기화

        // 마우스 위치가 맨 왼쪽(첫 번째) 또는 맨 오른쪽(마지막) 구역에 있는지 확인
        if (cornerPosition.x <= sectionWidth)                           // 맨 왼쪽 코너이다
        {
            Debug.Log("왼쪽 코너 클릭");
            cornerPosition.x += xOffset;                                // 왼쪽에서 클릭 시 중앙 쪽으로 이동
            if (cornerPosition.y >= screenHeight - sectionHeight)       // 맨 왼쪽이면서 맨 위쪽이다
            {
                cornerPosition.y -= yOffset * 8;                        // 가려져서 8배 함
                upgradeText.rectTransform.localPosition = changeUpgrade;       // upgradeText의 위치 조정
            }
            else if(cornerPosition.y <= sectionHeight)                  // 맨 왼쪽이면서 아래쪽이다
            {
                cornerPosition.y += yOffset * 8;
            }
        }
        else if (cornerPosition.x >= screenWidth - sectionWidth)        // 맨 오른쪽 코너이다
        {
            Debug.Log("오른쪽 코너 클릭");
            cornerPosition.x -= xOffset * 32;                                // 오른쪽에서 클릭 시 중앙 쪽으로 이동
            if(cornerPosition.y >= screenHeight - sectionHeight)        // 맨 오른쪽이면서 위쪽이다
            {
                cornerPosition.y -= yOffset * 8;
                upgradeText.rectTransform.localPosition = changeUpgrade;       // upgradeText의 위치 조정
            }
            else if(cornerPosition.y <= sectionHeight)                  // 맨 오른쪽이면서 아래쪽이다
            {
                cornerPosition.y += yOffset * 8;
            }
        }

        // 마우스 위치가 맨 위쪽 또는 맨 아래쪽 구역에 있는지 확인
        else if (cornerPosition.y >= screenHeight - sectionHeight)      // 맨 위쪽 코너이다
        {
            Debug.Log("위쪽 코너 클릭");
            cornerPosition.y -= yOffset * 8;                                // 위쪽에서 클릭 시 중앙 쪽으로 이동
            upgradeText.rectTransform.localPosition = changeUpgrade;       // upgradeText의 위치 조정
        }
        else if(cornerPosition.y >= screenHeight - sectionHeight * 2)   // 위에서 2번째 이다
        {
            Debug.Log("위에서 2번째 클릭");
            upgradeText.rectTransform.localPosition = changeUpgrade;       // upgradeText의 위치 조정
        }
        else if (cornerPosition.y <= sectionHeight)                     // 맨 아래쪽 코너이다
        {
            Debug.Log("아래쪽 코너 클릭");
            cornerPosition.y += yOffset * 8;                                // 아래쪽에서 클릭 시 중앙 쪽으로 이동
        }


        // 스크린 좌표를 RectTransform의 로컬 좌표로 변환
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            cornerPosition,
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
        onUpgradeYesButton?.Invoke();
        player.upgradeAble = false;     // 무슨 버튼을 누르던 강화가능 변수는 false 가 되어야 함
    }

    /// <summary>
    /// No 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    private void NoUpgrade()
    {
        SetActiveFalse();
        player.boardClickAble = true;
        player.upgradeAble = false;     // 무슨 버튼을 누르던 강화가능 변수는 false 가 되어야 함

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
    /// D 버튼을 눌러 설치된 solider를 파괴하는 함수
    /// </summary>
    private void DestroyFC()
    {
        SetActiveFalse();
        player.boardClickAble = true;
        player.upgradeAble = false;     // 무슨 버튼을 누르던 강화가능 변수는 false 가 되어야 함
        onDestroyButton?.Invoke(true);
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
        descriptionImage.gameObject.SetActive(true);
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
        descriptionImage.gameObject.SetActive(false);
        onAlphZero?.Invoke();
    }
}
