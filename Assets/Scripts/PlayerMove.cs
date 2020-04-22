using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; //최대 속도 제한
    public float jumpPower; // 점프속도
    Rigidbody2D rigid; //리지드보디 불러오기
    SpriteRenderer spriteRenderer; // 스프라이트2D 제어
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update() //단발적인 키 입력
    {
        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("Player_jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("Player_jump", true);
        }


        // 키를 떗을떄 속도 감속
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //스프라이트 플립 변경
        if(Input.GetButtonDown("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 에니메이션
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("Player_walk", false);
        else
            anim.SetBool("Player_walk", true);
    }

    void FixedUpdate() //지속적인 키 입력
    {
        //조작
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
}
