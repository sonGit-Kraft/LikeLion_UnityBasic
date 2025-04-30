using UnityEngine;

public class Enemy_Skeleton : Entity
{
    private bool isAttacking;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                // 추적
                rb.linearVelocity = new Vector2(moveSpeed * facingDir * 1.5f, rb.linearVelocity.y);

                Debug.Log("플레이어를 봤다");
                isAttacking = false;
            }
            else
            {
                // 공격
                Debug.Log("공격! " + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }

        if (!isGrounded || isWallDetected)
            Flip();

        Movement();
    }

    private void Movement()
    {
        // 공격이 아닐 때 움직임
        if(!isAttacking)
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
