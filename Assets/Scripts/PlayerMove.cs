using Packages.Rider.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed; //최대 속도 제한
    public float jumpPower; // 점프속도
    Rigidbody2D rigid; //리지드보디 불러오기
    SpriteRenderer spriteRenderer; // 스프라이트2D 제어
    Animator anim;
    CapsuleCollider2D capsuleCollider;
   /* private int jumpCount = 0;
    [SerializeField] int jumpMaxCount = 2;*/

    // 버튼 환경용 변수 셋팅
    public bool inputLeft = false;
    public bool inputRight = false;
    public bool inputJump = false;

    //플레이어 이동시 사용할 변수
    public bool nextSOF = false;
    public bool nextSOS = false;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Next")
        {
            //다음챕터
            gameManager.NextStage();

        }
        else if (collision.gameObject.tag == "FObject")
        {
            gameManager.ChageStage();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //게임종료
        }

        if(collision.gameObject.tag == "Trap")
        {
            OnDie();
            Time.timeScale = 0f;
            gameManager.GameOverScreen.SetActive(true);
            Debug.Log("함정");
        }

        if(collision.gameObject.tag == "Trigger1")
        {
            nextSOF = true;
            collision.enabled = false;
            Debug.Log("1활성화됨 " + collision.enabled + " " + nextSOF + " " + nextSOS);
            if ((nextSOF && nextSOS) == true)
            {
                gameManager.InsertStage(0);
            }
        }
        if(collision.gameObject.tag == "Trigger2")
        {
            nextSOS = true;
            collision.enabled = false;
            Debug.Log("2활성화됨 " + collision.enabled + " " + nextSOF + " " + nextSOS);
            if ((nextSOF && nextSOS) == true)
            {
                gameManager.InsertStage(0);
            }
        }
    }

    void Update() //단발적인 키 입력
    {
        //점프
        if ((Input.GetButton("Jump") || inputJump) && !anim.GetBool("Player_jump"))
        {
            inputJump = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("Player_jump", true);
        }

        // 키를 떗을떄 속도 감속
        if (Input.GetButtonUp("Horizontal") || (inputLeft || inputRight)) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.4f, rigid.velocity.y);
        }

        // PC 스프라이트 플립 변경
        if (Input.GetButton("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 버튼 스프라이트 플립 변경
        // 버튼 조작후 키보드 조작시 스프라이트 플립이 반대가 되는경우가 있는데 같은 사용하는 경우는 없으니 무시
        if (inputLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(inputRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }


        // 에니메이션
        // 에니메이션이 안풀리는 버그 발생함.
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("Player_walk", false);
        else
            anim.SetBool("Player_walk", true);

        //버튼 조작
        if (inputLeft)
        {
            rigid.AddForce(Vector2.left * 2, ForceMode2D.Impulse);
        }
        else if (inputRight)
        {
            rigid.AddForce(Vector2.right * 2, ForceMode2D.Impulse);
        }

    }

    void FixedUpdate() //지속적인 키 입력
    {
        //PC 조작
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);


        if (rigid.velocity.x > maxSpeed) //오른쪽 최대 속도 제한
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1)) //왼쪽 최대 속도 제한
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        // 착지 플렛폼
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f) { 
                    anim.SetBool("Player_jump", false);
                }
                // 바닥착지 확인
                //Debug.Log(rayHit.collider.name);
            }
        }
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }



}
