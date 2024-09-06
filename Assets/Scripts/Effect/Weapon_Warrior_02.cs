using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Warrior_02 : RecycleObject
{
    /// <summary>
    /// 이 이펙트의 수명
    /// </summary>
    public float lifeTime = 0.5f;

    /// <summary>
    /// 오디오소스
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// 오디오 시작 지점 (예: 1.5초 지점에서 시작)
    /// </summary>
    public float startTime = 0.1f;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));

        audioSource.time = startTime; // 지정된 시간 지점에서부터 재생
        audioSource.Play(); // Play on Awake 대신 수동으로 재생
    }
}
