using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //이동 속도
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator pAni;
    public float sprint = 4f; // 대시 속도 배율
    public float sprintDuration = 1f; // 달리기 지속 시간 (1초)
    public float sprintCooldown = 5f; // 대시 쿨다운 시간 (5초)

    private Rigidbody2D rb;  
    private bool isGrounded;
    private float currentSpeed = 5f; // 현재 이동 속도
    private float sprintTimer; // 대시 타이머
    private float sprintStart; // 대시 시작 시간
    private bool isSprinting = false; //달리기 중인지 여부
    private bool canSprint = true; // 대시 가능 여부

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        pAni.SetBool("move", false);
        sprintTimer = 0f; // 타이머 초기화
        currentSpeed = moveSpeed;
    }
   
    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if(!canSprint)
        {
            sprintTimer += Time.deltaTime;
            if(sprintTimer >= sprintCooldown)
            {
                canSprint = true;
                sprintTimer = 0f;

            }
        }

        if(isSprinting && Time.time - sprintStart >= sprintDuration)
        {
            isSprinting = false;
            pAni.SetBool("sprint", false);
            moveSpeed = currentSpeed ;
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && canSprint)
        {
            isSprinting = true;
            sprintStart = Time.time;
            moveSpeed = currentSpeed  * sprint;
            pAni.SetBool("sprint", true);
            canSprint = false;
        }
        

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //방향전환
        if (moveInput < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        if (moveInput > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
           

        if (moveInput < 0)
        {
          pAni.SetBool("move", true);
        }
        else if (moveInput > 0)
        {
         pAni.SetBool("move", true);

        }
        else
        {
        pAni.SetBool("move", false);
        }
         
           
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("JumpAction");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
