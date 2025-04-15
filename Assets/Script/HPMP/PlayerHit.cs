using System.Collections;
using UnityEngine;

public class PlayerHit : Entity
{

    public GameObject bullet;
    public Transform pos;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Entity�� ���ǵǾ� �ִ� Setup() �޼ҵ� ȣ��
        base.Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        //�⺻ ����
        if(Input.GetKeyDown("1"))
        {
            TakeDamage(20);
        }
        //��ų ����
        else if (Input.GetKeyDown("2"))
        {
            MP -= 100;
            target.TakeDamage(55);
        }
        else if (Input.GetMouseButtonDown(0) && MP >= 50)
        {
            Instantiate(bullet, pos.position, transform.rotation);
            MP -= 50;
        }
    }
    //�⺻ ü�� + ���� ���ʽ� + ���� ��� ���� ���
    public override float MaxHP => MaxHPBasic + MaxHPAttrBonus;
    //100+ ���緹�� * 30
    public float MaxHPBasic => 100 + 1 * 30;
    //�� * 10
    public float MaxHPAttrBonus => 10 * 10;

    public override float HPRecovery => 10;
    public override float MaxMP => 200;
    public override float MPRecovery => 25;

    public override void TakeDamage(float damage)
    {
        HP -= damage;

        StartCoroutine("HitAnimation");
    }

    private IEnumerator HitAnimation()
    {
        Color color = spriteRenderer.color;

        color.a = 0.2f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.1f);

        color.a = 1;
        spriteRenderer.color = color;

    }
}
