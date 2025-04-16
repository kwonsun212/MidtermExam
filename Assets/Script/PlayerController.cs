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

    [Header("점프 개선 설정")]
    public float falMultiplier = 2.5f;          //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;      //짧은 점프 배율


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

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //이동 방향 벡터
        Vector2 movement = new Vector3(moveHorizontal, 0, moveVertical);    //이동 방향 감지

        //속도로 직접 이동
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //착시 점프 높이 구현
        if (rb.velocity.y < 0)
        {
            //하강 시 중력 강화
            rb.velocity += Vector2.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime; //하강 시 중력 강화
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; // 상승중 점프 버튼을 때면 낮게 점프

        }



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
        transform.Translate(new Vector3(Mathf.Abs(moveInput) * Time.deltaTime, 0, 0));
        if(moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }


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
