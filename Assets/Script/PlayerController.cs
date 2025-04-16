using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; 
    public float jumpForce = 10f; 
    public float JumpIncrease = 4.0f; 
    public float JumpDuration = 5.0f;

    private float originalJumpForce;
    private float boostTimer = 0f;
    private bool isBoosted = false;

    public GameObject JumpTimerUI; 
    public TextMeshProUGUI JumpTimerText;    

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator pAni;
    public float sprint = 4f;
    public float sprintDuration = 1f; 
    public float sprintCooldown = 5f;

    private Rigidbody2D rb;  
    private bool isGrounded;

    private float currentSpeed = 5f; 
    private float sprintTimer; 
    private float sprintStart; 
    private bool isSprinting = false; 
    private bool canSprint = true; 

    [Header("점프 개선 설정")]
    public float falMultiplier = 2.5f;          
    public float lowJumpMultiplier = 2.0f;

    [Header("스피드 버프")]
    public float speedIncrease = 3.0f;
    public float speedDuration = 5.0f;

    private float originalMoveSpeed;
    private float speedTimer = 0f;
    private bool isSpeedBoosted = false;

    public GameObject SpeedTimerUI;
    public TextMeshProUGUI SpeedTimerText;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        pAni.SetBool("move", false);
        sprintTimer = 0f;
        currentSpeed = moveSpeed;

        originalJumpForce = jumpForce;
        originalMoveSpeed = moveSpeed;

        if (JumpTimerUI != null)
            JumpTimerUI.SetActive(false);

        if (SpeedTimerUI != null)
            SpeedTimerUI.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isBoosted)
        {
            boostTimer += Time.deltaTime;

            float timeLeft = Mathf.Clamp(JumpDuration - boostTimer, 0f, JumpDuration);
            if (JumpTimerText != null)
            {
                JumpTimerText.text = timeLeft.ToString("F1");
            }

            if (boostTimer >= JumpDuration)
            {
                jumpForce = originalJumpForce;
                boostTimer = 0f;

                if (JumpTimerUI != null)
                    JumpTimerUI.SetActive(false);

                isBoosted = false;
            }

        }

        if (isSpeedBoosted)
        {
            speedTimer += Time.deltaTime;

            float timeLeft = Mathf.Clamp(speedDuration - speedTimer, 0f, speedDuration);
            if (SpeedTimerText != null)
                SpeedTimerText.text = timeLeft.ToString("F1");

            if (speedTimer >= speedDuration)
            {
                moveSpeed = originalMoveSpeed;
                speedTimer = 0f;
                isSpeedBoosted = false;

                if (SpeedTimerUI != null)
                    SpeedTimerUI.SetActive(false);
            }
        }




        float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");


            Vector2 movement = new Vector3(moveHorizontal, 0, moveVertical);


            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);


            if (rb.velocity.y < 0)
            {

                rb.velocity += Vector2.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

            }



            float moveInput = Input.GetAxisRaw("Horizontal");

            if (!canSprint)
            {
                sprintTimer += Time.deltaTime;
                if (sprintTimer >= sprintCooldown)
                {
                    canSprint = true;
                    sprintTimer = 0f;

                }
            }

            if (isSprinting && Time.time - sprintStart >= sprintDuration)
            {
                isSprinting = false;
                pAni.SetBool("sprint", false);
                moveSpeed = currentSpeed;
            }

            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && canSprint)
            {
                isSprinting = true;
                sprintStart = Time.time;
                moveSpeed = currentSpeed * sprint;
                pAni.SetBool("sprint", true);
                canSprint = false;
            }


            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            
            transform.Translate(new Vector3(Mathf.Abs(moveInput) * Time.deltaTime, 0, 0));
            if (moveInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (moveInput < 0)
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

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
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
        if (collision.CompareTag("Item_Speed"))
        {
            moveSpeed = originalMoveSpeed + speedIncrease;
            isSpeedBoosted = true;
            speedTimer = 0f;

            if (SpeedTimerUI != null)
                SpeedTimerUI.SetActive(true);

            Destroy(collision.gameObject);
        }

    }
}
