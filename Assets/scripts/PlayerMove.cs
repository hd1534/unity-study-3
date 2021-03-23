using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public int jumpAble;
    public int jumpCount;

    public GameManager gameManager;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    int PlayerDamegedLayer;
    int PlayerLayer;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;

    private void Awake() {
        PlayerDamegedLayer = LayerMask.NameToLayer("PlayerDameged");
        PlayerLayer = LayerMask.NameToLayer("Player");

        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();     
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            PlaySound("JUMP");
        }

        // 멈출때 속도
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
        }

        // 캐릭터 이미지 방향 전환
        if (Input.GetButton("Horizontal"))
           spriteRenderer.flipX = (Input.GetAxisRaw("Horizontal") == -1);

        // 애니메이션 전환 (달리기 <-> 대기)
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
            animator.SetBool("isRunning", false);
        else
            animator.SetBool("isRunning", true);
    }

    void FixedUpdate() {
        // 좌우 움직이기
        rigid.AddForce(
            Vector2.right * Input.GetAxisRaw("Horizontal"),
            ForceMode2D.Impulse
        );

        // 속도 제한
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < -maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // 점프하고 내려올시 바닥에 닫는지 확인
        if (jumpCount > 0) {
            if (rigid.velocity.y < 0) {
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));  // 에디터 상에서 Ray를 그려주는 디버깅용 함수
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
                if (rayHit.collider != null) {
                    if (rayHit.distance < 0.9f) {
                        animator.SetBool("isJumping", false);
                        jumpCount = 0;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Item"){
            // 점수 추가
            if (other.gameObject.name.Contains("Bronze"))
                gameManager.stageScore += 50;
            else if (other.gameObject.name.Contains("Silver"))
                gameManager.stageScore += 100;
            else
                gameManager.stageScore += 150;

            PlaySound("ITEM");

            // 사라지게
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Finish") {
            PlaySound("FINISH");

            // 다음 스테이지
            gameManager.NextStage();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // 적과 부딪힐시
        if (other.gameObject.tag == "Enemy") {
            if (transform.position.y > other.transform.position.y)
                OnAttack(other.transform);
            else
                OnDamaged(other.transform.position);
        }
        // 스파이크와 부딪힐시
        if(other.gameObject.tag == "Spikes")
            OnDamaged(other.transform.position);
    }

    void OnAttack(Transform enemy) {
        PlaySound("ATTACK");

        // 점수 추가
        gameManager.stageScore += 200;

        // 반발력
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        
        // EnemyMove 스크립트의 함수 실행
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos) {
        PlaySound("DAMAGED");

        // 체력 깎기
        gameManager.HealthDown();

        // 레이어 바꾸기
        gameObject.layer = PlayerDamegedLayer;

        // 살짝 투명하게
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 튕겨 나가기
        rigid.AddForce(
            new Vector2(transform.position.x - targetPos.x > 0 ? 2 : -2, 1) * 7,
            ForceMode2D.Impulse
        );

        // 애니메이션
        animator.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
    }

    void OffDamaged() {
        gameObject.layer = PlayerLayer;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie() {
        PlaySound("DIE");

        // 레이어 바꾸기
        gameObject.layer = PlayerDamegedLayer;

        // 살짝 투명하게
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 뒤집기
        spriteRenderer.flipY = true;

        // Collider 비활성화
        capsuleCollider2D.enabled = false;

        // 위로 튕기기
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 5);

        // 애니메이션
        animator.SetTrigger("doDamaged");
    }
    public void Reposition() {
        rigid.velocity = Vector2.zero;
        transform.position = new Vector3(0, 2, 0);
    }

    void PlaySound(string name) {
        // 오디오 선택
        switch (name) {
            case "JUMP" :
                audioSource.clip = audioJump;
                break;
            case "ATTACK" :
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED" :
                audioSource.clip = audioDamaged;
                break;
            case "ITEM" :
                audioSource.clip = audioItem;
                break;
            case "DIE" :
                audioSource.clip = audioDie;
                break;
            case "FINISH" :
                audioSource.clip = audioFinish;
                break;
        }

        // 재생
        audioSource.Play();
    }
}
