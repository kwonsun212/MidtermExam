using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown("3") )
        {
            int damage = Random.Range(1, 100);
            target.TakeDamage(damage);
        }
    }

    public override float MaxHP => 200;
    public override float HPRecovery => 0;
    public override float MaxMP => 0;
    public override float MPRecovery => 0;

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
