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
    LightningPool lightning;                            // 마법사 공격
    Weapon_Barbarian_01_Pool weapon_Barbarian_01;       // 바바리안 공격
    Weapon_Barbarian_02_Pool weapon_Barbarian_02;
    Weapon_Warrior_01_Pool weapon_Warrior_01;           // 전사 공격
    Weapon_Warrior_02_Pool weapon_Warrior_02;

    /// <summary>
    /// 씬이 로딩 완료될 때마다 실행되는 초기화 함수
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        // 풀 컴포넌트 찾고, 찾으면 초기화하기

        // 번개
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

        // 바바리안 무기 1
        weapon_Barbarian_01 = GetComponentInChildren<Weapon_Barbarian_01_Pool>();
        if (weapon_Barbarian_01 != null)
        {
            Debug.Log("Weapon_Barbarian_01_Pool 초기화");
            weapon_Barbarian_01.Initialize();
        }
        else
        {
            Debug.LogError("Weapon_Barbarian_01_Pool 찾을 수 없습니다.");
        }

        // 바바리안 무기 2
        weapon_Barbarian_02 = GetComponentInChildren<Weapon_Barbarian_02_Pool>();
        if (weapon_Barbarian_02 != null)
        {
            Debug.Log("Weapon_Barbarian_02_Pool 초기화");
            weapon_Barbarian_02.Initialize();
        }
        else
        {
            Debug.LogError("Weapon_Barbarian_02_Pool 찾을 수 없습니다.");
        }

        // 전사 무기 1
        weapon_Warrior_01 = GetComponentInChildren<Weapon_Warrior_01_Pool>();
        if (weapon_Warrior_01 != null)
        {
            Debug.Log("Weapon_Warrior_01_Pool 초기화");
            weapon_Warrior_01.Initialize();
        }
        else
        {
            Debug.LogError("Weapon_Warrior_01_Pool 찾을 수 없습니다.");
        }

        // 전사 무기 2
        weapon_Warrior_02 = GetComponentInChildren<Weapon_Warrior_02_Pool>();
        if (weapon_Warrior_02 != null)
        {
            Debug.Log("Weapon_Warrior_02_Pool 초기화");
            weapon_Warrior_02.Initialize();
        }
        else
        {
            Debug.LogError("Weapon_Warrior_02_Pool 찾을 수 없습니다.");
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
            case PoolObjectType.Lightning:                                      // 번개
                result = lightning.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.Weapon_Barbarian_01:                            // 바바리안 무기 1
                result = weapon_Barbarian_01.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.Weapon_Barbarian_02:                            // 바바리안 무기 2
                result = weapon_Barbarian_02.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.Weapon_Warrior_01:                              // 전사 무기 1
                result = weapon_Warrior_01.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.Weapon_Warrior_02:                              // 전사 무기 2
                result = weapon_Warrior_02.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    // 번개 --------------------------------------------------------------------------------------------------------------------------

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

    // 바바리안 --------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 바바리안 무기 1 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Weapon_Barbarian_01 GetWeapon_Barbarian_01()
    {
        return weapon_Barbarian_01.GetObject();
    }

    /// <summary>
    /// 바바리안 무기 1 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Weapon_Barbarian_01 GetWeapon_Barbarian_01(Vector3 position, float angle = 0.0f)
    {
        return weapon_Barbarian_01.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 바바리안 무기 2 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Weapon_Barbarian_02 GetWeapon_Barbarian_02()
    {
        return weapon_Barbarian_02.GetObject();
    }

    /// <summary>
    /// 바바리안 무기 2 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Weapon_Barbarian_02 GetWeapon_Barbarian_02(Vector3 position, float angle = 0.0f)
    {
        return weapon_Barbarian_02.GetObject(position, angle * Vector3.forward);
    }

    // 전사 --------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 전사 무기 1 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Weapon_Warrior_01 GetWeapon_Warrior_01()
    {
        return weapon_Warrior_01.GetObject();
    }

    /// <summary>
    /// 전사 무기 1 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Weapon_Warrior_01 GetWeapon_Warrior_01(Vector3 position, float angle = 0.0f)
    {
        return weapon_Warrior_01.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 전사 무기 2 하나 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public Weapon_Warrior_02 GetWeapon_Warrior_02()
    {
        return weapon_Warrior_02.GetObject();
    }

    /// <summary>
    /// 전사 무기 2 하나 가져와서 특정 위치에 배치하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Weapon_Warrior_02 GetWeapon_Warrior_02(Vector3 position, float angle = 0.0f)
    {
        return weapon_Warrior_02.GetObject(position, angle * Vector3.forward);
    }
}
