using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Entity�� ���ǵǾ� �ִ� Setup() �޼ҵ� ȣ��
        base,Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        //�⺻ ����
        if(Input.GetKeyDown("1"))
        {
            target.TakeDamage(20);
        }
        //��ų ����
        else if (Input.GetKeyDown("2"))
        {
            MP -= 100;
            target,TakeDamage(55);
        }
    }

    public override float MaxHP => MaxHPBasic + MaxHPAttrBonus;



}
