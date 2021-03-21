using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public int jumpAble;
    public int jumpCount;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();     
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        // 점프
        if (jumpCount < jumpAble && Input.GetButtonDown("Jump")) {
            jumpCount++;
            animator.SetBool("isJumping", true);
            rigid.AddForce(
                Vector2.up * jumpPower,
                ForceMode2D.Impulse
            );
        }

        // 멈출때 속도
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
        }

        // 캐릭터 이미지 방향 전환
        if(Input.GetButtonDown("Horizontal"))
           spriteRenderer.flipX = (Input.GetAxisRaw("Horizontal") == -1);

        // 애니메이션 전환 (달리기 <-> 대기)
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
            animator.SetBool("isRunning", false);
        else
            animator.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        // 좌우 움직이기
        rigid.AddForce(
            Vector2.right * Input.GetAxisRaw("Horizontal"),
            ForceMode2D.Impulse
        );

        // 속도 제한
        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if(rigid.velocity.x < -maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // 점프하고 내려올시 바닥에 닫는지 확인
        if(jumpCount > 0) {
            if(rigid.velocity.y < 0) {
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));  // 에디터 상에서 Ray를 그려주는 디버깅용 함수
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
                if(rayHit.collider != null) {
                    if(rayHit.distance < 0.9f) {
                        animator.SetBool("isJumping", false);
                        jumpCount = 0;
                    }
                }
            }
        }
    }
}
