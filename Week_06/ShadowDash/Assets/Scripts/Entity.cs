using UnityEngine;

// Entity
// 게임 오브젝트, NPC, 플레이어 등을 지칭하는 개체
// 예: "이 게임의 모든 캐릭터는 하나의 엔티티(entity) 시스템을 통해 관리된다."

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance; // 바닥 체크 거리
    [SerializeField] protected LayerMask whatIsGround; // 바닥으로 인식할 레이어
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance; // 바닥 체크 거리
    protected bool isGrounded; // 현재 바닥에 있는지 여부
    protected bool isWallDetected; // 현재 바닥에 있는지 여부

    protected int facingDir = 1; // 캐릭터의 바라보는 방향 (1: 오른쪽, -1: 왼쪽)
    protected bool facingRight = true; // 현재 오른쪽을 바라보고 있는지 여부

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();    
    }

    protected virtual void Update()
    {
        CollisionCheck(); // 충돌 감지
    }

    // 바닥 충돌 체크 함수
    protected virtual void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGround);
    }

    // 캐릭터의 방향을 변경하는 함수
    protected virtual void Flip()
    {
        facingDir *= -1; // 방향 반전
        facingRight = !facingRight; // 방향 상태 반전
        transform.Rotate(0, 180, 0); // 캐릭터 회전
    }

    // 기즈모를 이용해 바닥 체크 거리 표시
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
