using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public int nextMove;

    float thinkTime;
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Think();
    }

    private void FixedUpdate() {
        // 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 낭떠러지이면 방향을 바꾸고 다시 invoke
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(1,0,0));  // 에디터 상에서 Ray를 그려주는 디버깅용 함수
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));
        if(rayHit.collider == null) {
            nextMove *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            CancelInvoke("Think"); // 값을 안주면 모든 예약을 취소함
            Invoke("Think", thinkTime);
        }
    }

    private void Think() {
        // 다음 움직임 (랜덤)
        nextMove = Random.Range(-1, 2);

        // 캐릭터 방향 및 애니메이션 전환
        if(nextMove == 0) {
            animator.SetBool("isRunning", false);
        }
        else if(nextMove > 0) {
            spriteRenderer.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else {
            spriteRenderer.flipX = false;
            animator.SetBool("isRunning", true);
        }       

        thinkTime = Random.Range(2, 5);
        Invoke("Think", thinkTime);
    }
}
