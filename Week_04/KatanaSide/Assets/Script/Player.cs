using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpUp = 1f;
    [SerializeField] private float power = 5f;
    [SerializeField] private GameObject slash;

    [Header("Shadow Settings")]
    [SerializeField] private GameObject shadowPrefab;
    private List<GameObject> shadows = new List<GameObject>();

    [Header("Effects")]
    [SerializeField] private GameObject hitLazer;
    [SerializeField] private GameObject jumpDust;
    [SerializeField] private GameObject wallDust;

    [Header("Wall Interaction")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float slidingSpeed = 0.5f;
    [SerializeField] private float wallJumpPower = 5f;

    private bool isJumping = false;
    private bool isWallJumping = false;
    private bool isWall = false;
    private float directionMultiplier = 1f;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private const float SHADOW_MAX_COUNT = 6;
    private const float WALL_DUST_LIFETIME = 0.3f;

    private Vector3 direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Vector2.zero;
    }

    void Update()
    {
        if (!isWallJumping)
        {
            HandleInput();
            Move();
        }

        CheckWallCollision();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        HandleGroundCheck();
    }

    private void HandleInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");

        if (direction.x != 0)
        {
            HandleMovementAnimation();
        }
        else
        {
            StopRunning();
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            PerformAttack();
        }
    }

    private void HandleMovementAnimation()
    {
        spriteRenderer.flipX = direction.x < 0;
        animator.SetBool("Run", true);
        directionMultiplier = direction.x < 0 ? -1 : 1;

        FlipShadows(spriteRenderer.flipX);
    }

    private void StopRunning()
    {
        animator.SetBool("Run", false);
        ClearShadows();
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
        Instantiate(hitLazer, transform.position, Quaternion.identity);
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void CheckWallCollision()
    {
        // 벽 충돌 감지
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * directionMultiplier, wallCheckDistance, wallLayer);
        animator.SetBool("Grab", isWall);

        if (isWall)
        {
            // 벽 슬라이딩 처리
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * slidingSpeed);

            // 벽 점프 처리
            if (Input.GetKeyDown(KeyCode.W))
            {
                PerformWallJump();
            }
        }
    }

    private void PerformWallJump()
    {
        isWallJumping = true;

        // 벽 먼지 효과 생성
        GameObject dust = Instantiate(wallDust, transform.position + new Vector3(0.8f * directionMultiplier, 0, 0), Quaternion.identity);
        dust.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        Destroy(dust, WALL_DUST_LIFETIME);

        // 벽 점프 실행
        rigidBody.velocity = new Vector2(-directionMultiplier * wallJumpPower, 0.9f * wallJumpPower);

        // 방향 전환
        spriteRenderer.flipX = !spriteRenderer.flipX;
        directionMultiplier = -directionMultiplier;

        // 벽 점프 상태 초기화
        Invoke(nameof(ResetWallJump), WALL_DUST_LIFETIME);
    }

    private void ResetWallJump()
    {
        isWallJumping = false;
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && !animator.GetBool("Jump"))
        {
            Jump();
            animator.SetBool("Jump", true);
            CreateJumpDust();
        }
    }

    private void Jump()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }

    private void HandleGroundCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigidBody.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        if (rigidBody.velocity.y < 0)
        {
            if (rayHit.collider != null && rayHit.distance < 0.7f)
            {
                animator.SetBool("Jump", false);
            }
            else
            {
                animator.SetBool("Jump", !isWall);
                animator.SetBool("Grab", isWall);
            }
        }
    }

    private void CreateJumpDust()
    {
        GameObject dust = isWall ? wallDust : jumpDust;
        Instantiate(dust, transform.position, Quaternion.identity);
    }

    private void FlipShadows(bool flipX)
    {
        foreach (var shadow in shadows)
        {
            shadow.GetComponent<SpriteRenderer>().flipX = flipX;
        }
    }

    private void ClearShadows()
    {
        foreach (var shadow in shadows)
        {
            Destroy(shadow);
        }
        shadows.Clear();
    }

    public void RunShadow()
    {
        if (shadows.Count < SHADOW_MAX_COUNT)
        {
            GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            shadow.GetComponent<Shadow>().TwSpeed = 10 - shadows.Count;
            shadows.Add(shadow);
        }
    }

    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f, 0), Quaternion.identity);
    }

    public void AttSlash()
    {
        GameObject slashInstance = Instantiate(slash, transform.position, Quaternion.identity);
        slashInstance.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

        Vector2 force = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        rigidBody.AddForce(force * power, ForceMode2D.Impulse);
    }
}