using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //�̵� �ӵ�
    public float jumpForce = 10f; //�⺻ ���� ��
    public float JumpIncrease = 4.0f; //���������� ������ ���� ��
    public float JumpDuration = 5.0f; //���� ���� �ð�

    private float originalJumpForce;
    private float boostTimer = 0f;
    private bool isBoosted = false;

    public GameObject JumpTimerUI; //���� UI ������Ʈ
    public TextMeshProUGUI JumpTimerText;     //���� UI ���� �ؽ�Ʈ 

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator pAni;
    public float sprint = 4f; // ��� �ӵ� ����
    public float sprintDuration = 1f; // �޸��� ���� �ð� (1��)
    public float sprintCooldown = 5f; // ��� ��ٿ� �ð� (5��)

    private Rigidbody2D rb;  
    private bool isGrounded;
    private float currentSpeed = 5f; // ���� �̵� �ӵ�
    private float sprintTimer; // ��� Ÿ�̸�
    private float sprintStart; // ��� ���� �ð�
    private bool isSprinting = false; //�޸��� ������ ����
    private bool canSprint = true; // ��� ���� ����

    [Header("���� ���� ����")]
    public float falMultiplier = 2.5f;          //�ϰ� �߷� ����
    public float lowJumpMultiplier = 2.0f;      //ª�� ���� ����



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        pAni.SetBool("move", false);
        sprintTimer = 0f; // Ÿ�̸� �ʱ�ȭ
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
                jumpForce = originalJumpForce; // ������ ������� ����
                isBoosted = false;
                boostTimer = 0f;

                if (JumpTimerUI != null)
                    JumpTimerUI.SetActive(false);


            }

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector2 movement = new Vector3(moveHorizontal, 0, moveVertical);    //�̵� ���� ����

        //�ӵ��� ���� �̵�
        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //���� ���� ���� ����
        if (rb.velocity.y < 0)
        {
            //�ϰ� �� �߷� ��ȭ
            rb.velocity += Vector2.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime; //�ϰ� �� �߷� ��ȭ
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; // ����� ���� ��ư�� ���� ���� ����

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

        //������ȯ
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
