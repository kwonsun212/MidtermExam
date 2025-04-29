using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : Entity
{
    float score;

    private SpriteRenderer spriteRenderer;
    private Transform player;

    public float moveSpeed = 1.5f;        // ������� �ӵ�
    public float minDistance = 2.0f;      // �ּ� �Ÿ� (�̺��� ��������� ����)

    public GameObject enemyTracePrefab;
    public float spawnRadius = 2f;      // ��ȯ�� �ֺ� �ݰ�
    public float spawnInterval = 10f;    // ��ȯ ���� (��)


    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Setup();

        StartCoroutine(SpawnEnemyTraceRepeatedly());

        score = 1000f;
    }

    // Update is called once per frame
    private void Update()
    {
        FollowPlayerWithDistance();

        if (Input.GetKeyDown("3"))
        {
            int damage = Random.Range(1, 100);
            target.TakeDamage(damage);
        }

        score -= Time.deltaTime;


    }

    void FollowPlayerWithDistance()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // ���� �Ÿ� �̻��� ���� ���󰣴�
        if (distance > minDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    public override float MaxHP => 200;
    public override float HPRecovery => 0;
    public override float MaxMP => 0;
    public override float MPRecovery => 0;

    public override void TakeDamage(float damage)
    {
        HP -= damage;


        if (HP <= 0)
        {
            HighScore.Tryset(SceneManager.GetActiveScene().buildIndex, (int)score);


            SceneManager.LoadScene("Ending");
            Destroy(gameObject); // HP�� 0 ���ϰ� �Ǹ� ������Ʈ ����
            return;
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
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject); // �Ѿ˵� ����
        }
    }
    private IEnumerator SpawnEnemyTraceRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // spawnInterval ��ŭ ���

            // EnemyTrace �������� �Ҵ�Ǿ� �ְ�, ������ Ȯ���� ���� ��ȯ
            if (enemyTracePrefab != null)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

                // ���� ��ȯ ��ġ�� Enemy�� ���� Y�� ��ġ�� �������� ���� offset
                Vector3 spawnPosition = transform.position + Vector3.up * spawnOffset.magnitude;

                Instantiate(enemyTracePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
