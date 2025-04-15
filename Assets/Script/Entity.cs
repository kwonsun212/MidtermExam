using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Stats stats;    //캐릭터 정보 
    public Entity target;   //공격 대상

    //체력(HP) 프로퍼티: 0~MaxHP 사이의 값을 넘어갈 수 없도록 설정
    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;

    }

    public float MP
    {
        set => stats.MP = Mathf.Clamp(value, 0, MaxMP);
        get => stats.MP;

    }


    //현재 프로퍼티는 추상(abstract)으로 선언
    //실제 작동하는 내용은 플레이어, 적과 같은 파생 클래스에서 정의
    public abstract float MaxHP { get; }        //최대 체력
    public abstract float HPRecovery { get; }   //초당 체력 회복량
    public abstract float MaxMP { get; }        //최대 마나
    public abstract float MPRecovery { get; }   //초당 마나 회복량

    protected void setup()
    {
        HP = MaxHP;
        MP = MaxMP;

        StartCoroutine("Recovery");
    }


    /// <summary>
    /// 초당 체력,마나 회복
    /// </summary>
    protected IEnumerator Recovery()
    {
        while (true)
        {
            if (HP < MaxHP) HP += HPRecovery;
            if (MP < MaxMP) MP += MPRecovery;

            yield return new WaitForSeconds(1);
        }
    }

    // 상대방을 공격할 때 상대방의 TakeDamage() 호출
    // 매개변수 damage는 공격하는 본인 공격력
    public abstract void TakeDamage(float damage);
}

[System.Serializable]
public struct Stats
{
    //이름,레벨,직업,스텟(힘,지능 등),체력/마나,경험치 등의 캐릭터 수치


    [HideInInspector]
    public float HP;
    [HideInInspector]
    public float MP;
}