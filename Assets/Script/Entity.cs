using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Stats stats;    //ĳ���� ���� 
    public Entity target;   //���� ���

    //ü��(HP) ������Ƽ: 0~MaxHP ������ ���� �Ѿ �� ������ ����
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


    //���� ������Ƽ�� �߻�(abstract)���� ����
    //���� �۵��ϴ� ������ �÷��̾�, ���� ���� �Ļ� Ŭ�������� ����
    public abstract float MaxHP { get; }        //�ִ� ü��
    public abstract float HPRecovery { get; }   //�ʴ� ü�� ȸ����
    public abstract float MaxMP { get; }        //�ִ� ����
    public abstract float MPRecovery { get; }   //�ʴ� ���� ȸ����

    protected void setup()
    {
        HP = MaxHP;
        MP = MaxMP;

        StartCoroutine("Recovery");
    }


    /// <summary>
    /// �ʴ� ü��,���� ȸ��
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

    // ������ ������ �� ������ TakeDamage() ȣ��
    // �Ű����� damage�� �����ϴ� ���� ���ݷ�
    public abstract void TakeDamage(float damage);
}

[System.Serializable]
public struct Stats
{
    //�̸�,����,����,����(��,���� ��),ü��/����,����ġ ���� ĳ���� ��ġ


    [HideInInspector]
    public float HP;
    [HideInInspector]
    public float MP;
}