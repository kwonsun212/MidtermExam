using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class PlayerHit : Entity
{

    public GameObject bullet;
    public Transform pos;


    private SpriteRenderer spriteRenderer;

    private PlayerController playerController;


    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

        //Entity�� ���ǵǾ� �ִ� Setup() �޼ҵ� ȣ��
        base.Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        //�⺻ ����
        if (Input.GetMouseButtonDown(0) && MP >= 25)
        {
            Instantiate(bullet, pos.position, transform.rotation);
            MP -= 25;
         }

    }
    

    //�⺻ ü�� + ���� ���ʽ� + ���� ��� ���� ���
    public override float MaxHP => MaxHPBasic + MaxHPAttrBonus;
    //100+ ���緹�� * 30
    public float MaxHPBasic => 100 ;
    //�� * 10
    public float MaxHPAttrBonus => 10 * 10;

    public override float HPRecovery => 10;
    public override float MaxMP => 200;
    public override float MPRecovery => 25;

    public override void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ����
        }

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            if (!playerController.isInvincible)
            {
                TakeDamage(40);
        
            }
            else
            {
                // ���� ���¿����� ������ ����
                Debug.Log("����");
            }
        }
    }
}
