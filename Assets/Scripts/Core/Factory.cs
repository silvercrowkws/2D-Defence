using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀을 사용하는 오브젝트의 종류
/// </summary>
public enum PoolObjectType
{
    Lightning = 0,
    Weapon_Barbarian_01,
    Weapon_Barbarian_02,
    Weapon_Warrior_01,
    Weapon_Warrior_02,
}

public class Factory : Singleton<Factory>
{
    // 오브젝트 풀들
    LightningPool lightning;

    /// <summary>
    /// 씬이 로딩 완료될 때마다 실행되는 초기화 함수
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        // 풀 컴포넌트 찾고, 찾으면 초기화하기
        lightning = GetComponentInChildren<LightningPool>();
        if (lightning != null)
        {
            Debug.Log("LightningPool 초기화");
            lightning.Initialize();
        }
        else
        {
            Debug.LogError("LightningPool을 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Lightning:
                result = lightning.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    /// <summary>
    /// Lightning 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Lightning GetLightning()
    {
        return lightning.GetObject();
    }

    /// <summary>
    /// 번개 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Lightning GetLightning(Vector3 position, float angle = 0.0f)
    {
        return lightning.GetObject(position, angle * Vector3.forward);
    }

    /*/// <summary>
    /// 번개 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Lightning GetLightning(Vector3 position)
    {
        return lightning.GetObject(position);
    }*/
}
