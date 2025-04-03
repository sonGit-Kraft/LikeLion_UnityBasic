using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트 참조
    private Animator animator; // Animator 컴포넌트 참조

    [SerializeField] private float moveSpeed; // 이동 속도
    [SerializeField] private float jumpForce; // 점프 힘

    private float xInput; // 수평 입력 값
    private int facingDir = 1; // 캐릭터의 바라보는 방향 (1: 오른쪽, -1: 왼쪽)
    private bool facingRight = true; // 현재 오른쪽을 바라보고 있는지 여부

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed; // 대쉬 속도
    [SerializeField] private float dashDuration; // 대쉬 지속 시간
    [SerializeField] private float dashTime; // 현재 대쉬 시간
    [SerializeField] private float dashCooldown; // 대쉬 재사용 대기 시간
    [SerializeField] private float dashCooldownTimer; // 현재 대쉬 쿨다운 시간

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance; // 바닥 체크 거리
    [SerializeField] private LayerMask whatIsGround; // 바닥으로 인식할 레이어
    private bool isGround; // 현재 바닥에 있는지 여부

    [Header("Attack Info")]
    [SerializeField] private float comboTime = 0.3f; // 콤보 유지 시간
    private float comboTimeCounter; // 콤보 시간 카운터
    private bool isAttacking; // 공격 중인지 여부
    private int comboCounter; // 콤보 횟수


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기
        animator = GetComponentInChildren<Animator>(); // Animator 가져오기
    }

    void Update()
    {
        CheckInput(); // 입력 처리
        Movement(); // 이동 처리
        CollisionCheck(); // 충돌 감지

        // 시간 감소 처리
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeCounter -= Time.deltaTime;

        FlipController(); // 방향 전환 처리
        AnimatorController(); // 애니메이션 처리
    }

    // 공격이 끝났을 때 호출되는 함수
    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2) // 최대 콤보 횟수를 넘으면 초기화
            comboCounter = 0;
    }

    // 바닥 충돌 체크 함수
    private void CollisionCheck()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    // 이동 처리 함수
    private void Movement()
    {
        if (isAttacking)
            rb.linearVelocity = new Vector2(0, 0); // 공격 중일 때 움직이지 않음
        else if (dashTime > 0)
            rb.linearVelocity = new Vector2(facingDir * dashSpeed, 0); // 대쉬 중일 때 y축 고정
        else
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y); // 일반 이동
    }

    // 점프 처리 함수
    private void Jump()
    {
        if (isGround) // 바닥에 있을 때만 점프 가능
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // 키 입력을 감지하는 함수
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // 좌우 입력 값 저장

        if (Input.GetKeyDown(KeyCode.Mouse0)) // 마우스 왼쪽 버튼 클릭 시 공격
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 입력 시 점프
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 Shift 입력 시 대쉬
        {
            DashAbility();
        }
    }

    // 공격 시작 처리 함수
    private void StartAttackEvent()
    {
        if (!isGround) // 공중에서 공격 불가능
            return;

        if (comboTimeCounter < 0) // 콤보 시간이 지나면 초기화
            comboCounter = 0;

        isAttacking = true; // 공격 상태로 변경
        comboTimeCounter = comboTime; // 콤보 시간 초기화
    }

    // 대쉬 처리 함수
    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking) // 공격 중이 아닐 때만 대쉬 가능
        {
            dashCooldownTimer = dashCooldown; // 대쉬 쿨다운 초기화
            dashTime = dashDuration; // 대쉬 지속 시간 설정
        }
    }

    // 애니메이션 상태 업데이트 함수
    private void AnimatorController()
    {
        bool isMoving = rb.linearVelocity.x != 0; // x축 이동 여부 확인
        animator.SetFloat("yVelocity", rb.linearVelocityY); // y축 속도 설정
        animator.SetBool("isMoving", isMoving); // 이동 여부 설정
        animator.SetBool("isGround", isGround); // 바닥 여부 설정
        animator.SetBool("isDashing", dashTime > 0); // 대쉬 여부 설정
        animator.SetBool("isAttacking", isAttacking); // 공격 여부 설정
        animator.SetInteger("comboCounter", comboCounter); // 콤보 횟수 설정
    }

    // 캐릭터의 방향을 변경하는 함수
    private void Flip()
    {
        facingDir *= -1; // 방향 반전
        facingRight = !facingRight; // 방향 상태 반전
        transform.Rotate(0, 180, 0); // 캐릭터 회전
    }

    // 방향 변경을 감지하고 처리하는 함수
    private void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight) // 오른쪽 이동 중이고 왼쪽을 보고 있다면 뒤집기
        {
            Flip();
        }
        else if (rb.linearVelocityX < 0 && facingRight) // 왼쪽 이동 중이고 오른쪽을 보고 있다면 뒤집기
        {
            Flip();
        }
    }

    // 기즈모를 이용해 바닥 체크 거리 표시
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
