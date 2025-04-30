using UnityEngine;

// Entity
// ���� ������Ʈ, NPC, �÷��̾� ���� ��Ī�ϴ� ��ü
// ��: "�� ������ ��� ĳ���ʹ� �ϳ��� ��ƼƼ(entity) �ý����� ���� �����ȴ�."

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance; // �ٴ� üũ �Ÿ�
    [SerializeField] protected LayerMask whatIsGround; // �ٴ����� �ν��� ���̾�
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance; // �ٴ� üũ �Ÿ�
    protected bool isGrounded; // ���� �ٴڿ� �ִ��� ����
    protected bool isWallDetected; // ���� �ٴڿ� �ִ��� ����

    protected int facingDir = 1; // ĳ������ �ٶ󺸴� ���� (1: ������, -1: ����)
    protected bool facingRight = true; // ���� �������� �ٶ󺸰� �ִ��� ����

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();    
    }

    protected virtual void Update()
    {
        CollisionCheck(); // �浹 ����
    }

    // �ٴ� �浹 üũ �Լ�
    protected virtual void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, whatIsGround);
    }

    // ĳ������ ������ �����ϴ� �Լ�
    protected virtual void Flip()
    {
        facingDir *= -1; // ���� ����
        facingRight = !facingRight; // ���� ���� ����
        transform.Rotate(0, 180, 0); // ĳ���� ȸ��
    }

    // ����� �̿��� �ٴ� üũ �Ÿ� ǥ��
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
