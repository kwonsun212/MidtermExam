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

        //Entity에 정의되어 있는 Setup() 메소드 호출
        base,Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        //기본 공격
        if(Input.GetKeyDown("1"))
        {
            target.TakeDamage(20);
        }
        //스킬 공격
        else if (Input.GetKeyDown("2"))
        {
            MP -= 100;
            target,TakeDamage(55);
        }
    }

    public override float MaxHP => MaxHPBasic + MaxHPAttrBonus;



}
