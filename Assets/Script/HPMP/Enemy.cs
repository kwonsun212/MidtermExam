using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : Entity
{
    float score;

    private SpriteRenderer spriteRenderer;
    private Transform player;

    public float moveSpeed = 1.5f;        // 따라오는 속도
    public float minDistance = 2.0f;      // 최소 거리 (이보다 가까워지지 않음)

    public GameObject enemyTracePrefab;
    public float spawnRadius = 2f;      // 소환할 주변 반경
    public float spawnInterval = 10f;    // 소환 간격 (초)


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

        // 일정 거리 이상일 때만 따라간다
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
            Destroy(gameObject); // HP가 0 이하가 되면 오브젝트 제거
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
            Destroy(collision.gameObject); // 총알도 제거
        }
    }
    private IEnumerator SpawnEnemyTraceRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // spawnInterval 만큼 대기

            // EnemyTrace 프리팹이 할당되어 있고, 설정된 확률에 따라 소환
            if (enemyTracePrefab != null)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

                // 최종 소환 위치는 Enemy의 현재 Y축 위치를 기준으로 위로 offset
                Vector3 spawnPosition = transform.position + Vector3.up * spawnOffset.magnitude;

                Instantiate(enemyTracePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
