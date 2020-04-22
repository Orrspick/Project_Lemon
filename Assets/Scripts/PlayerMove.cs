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
        if(Input.GetButtonDown("Jump"))
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

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
    }
}
