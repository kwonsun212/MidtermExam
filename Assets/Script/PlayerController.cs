using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //이동 속도
    public float jumpForce = 10f; //기본 점프 힘
    public float JumpIncrease = 4.0f; //아이템으로 증가할 점프 힘
    public float JumpDuration = 5.0f; //버프 지속 시간

    private float originalJumpForce;
    private float boostTimer = 0f;
    private bool isBoosted = false;

    public GameObject JumpTimerUI; //점프 UI 오브젝트
    public TextMeshProUGUI JumpTimerText;     //점프 UI 안의 텍스트 

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

        originalJumpForce = jumpForce;

        if (JumpTimerUI != null)
            JumpTimerUI.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isBoosted)
        {
            boostTimer += Time.deltaTime;

            float timeLeft = Mathf.Clamp(JumpDuration - boostTimer, 0f, JumpDuration);
            if (JumpTimerText != null)
                JumpTimerText.text = timeLeft.ToString("F1");



            if (boostTimer >= JumpDuration)
            {
                jumpForce = originalJumpForce; // 점프력 원래대로 복구
                isBoosted = false;
                boostTimer = 0f;

                if (JumpTimerUI != null)
                    JumpTimerUI.SetActive(false);


            }
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

        if(collision.CompareTag("Item_Jump"))
        {
            jumpForce = originalJumpForce + JumpIncrease;
            isBoosted = true;
            boostTimer = 0f;

            if (JumpTimerUI != null)
                JumpTimerUI.SetActive(true);

            Destroy(collision.gameObject);
        }

    }
}
