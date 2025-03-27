using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 속성")]
    public float speed = 5; // 이동 속도
    public float jumpUp = 1; // 점프 높이
    public float power = 5; // 공격 힘
    public Vector3 direction; // 이동 방향
    public GameObject slash; // 공격 효과

    // 그림자 효과
    public GameObject Shadow1;
    List<GameObject> sh = new List<GameObject>();

    // 히트 이펙트
    public GameObject hit_lazer;

    bool bJump = false;
    Animator pAnimator;
    Rigidbody2D pRig2D;
    SpriteRenderer sp;

    public GameObject Jdust; // 점프 먼지 효과

    // 벽 점프 관련 변수
    public Transform wallChk; 
    public float wallchkDistance; 
    public LayerMask wLayer;
    bool isWall;
    public float slidingSpeed; 
    public float wallJumpPower; 
    public bool isWallJump;
    float isRight = 1; 

    public GameObject walldust; // 벽 점프 먼지 효과

    void Start()
    {
        pAnimator = GetComponent<Animator>();
        pRig2D = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
        sp = GetComponent<SpriteRenderer>();
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");

        if (direction.x < 0)
        {
            sp.flipX = true;
            pAnimator.SetBool("Run", true);
            isRight = -1;
            FlipShadows();
        }
        else if (direction.x > 0)
        {
            sp.flipX = false;
            pAnimator.SetBool("Run", true);
            isRight = 1;
            FlipShadows();
        }
        else
        {
            pAnimator.SetBool("Run", false);
            ClearShadows();
        }

        if (Input.GetMouseButtonDown(0)) // 공격
        {
            pAnimator.SetTrigger("Attack");
            Instantiate(hit_lazer, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (!isWallJump)
        {
            KeyInput();
            Move();
        }

        // 벽 체크
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, wLayer);
        pAnimator.SetBool("Grab", isWall);

        if (Input.GetKeyDown(KeyCode.W) && !pAnimator.GetBool("Jump"))
        {
            Jump();
            pAnimator.SetBool("Jump", true);
            JumpDust();
        }

        if (isWall)
        {
            isWallJump = false;
            pRig2D.linearVelocity = new Vector2(pRig2D.linearVelocityX, pRig2D.linearVelocityY * slidingSpeed);

            if (Input.GetKeyDown(KeyCode.W)) // 벽 점프
            {
                isWallJump = true;
                CreateWallDust();
                Invoke("FreezeX", 0.3f);
                pRig2D.linearVelocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                sp.flipX = !sp.flipX;
                isRight = -isRight;
            }
        }
    }

    void FreezeX()
    {
        isWallJump = false;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(pRig2D.position, Vector3.down, new Color(0, 1, 0));

        // 땅 체크
        RaycastHit2D rayHit = Physics2D.Raycast(pRig2D.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (pRig2D.linearVelocityY < 0)
        {
            if (rayHit.collider != null && rayHit.distance < 0.7f)
            {
                pAnimator.SetBool("Jump", false);
            }
            else if (!isWall)
            {
                pAnimator.SetBool("Jump", true);
            }
            else
            {
                pAnimator.SetBool("Grab", true);
            }
        }
    }

    public void Jump()
    {
        pRig2D.linearVelocity = Vector2.zero;
        pRig2D.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }

    public void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void AttSlash()
    {
        Vector2 attackDirection = sp.flipX ? Vector2.left : Vector2.right;
        pRig2D.AddForce(attackDirection * power, ForceMode2D.Impulse);
        Instantiate(slash, transform.position, Quaternion.identity);
    }

    void FlipShadows()
    {
        foreach (var shadow in sh)
        {
            shadow.GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
    }

    void ClearShadows()
    {
        foreach (var shadow in sh)
        {
            Destroy(shadow);
        }
        sh.Clear();
    }

    public void RunShadow()
    {
        if (sh.Count < 6)
        {
            GameObject go = Instantiate(Shadow1, transform.position, Quaternion.identity);
            go.GetComponent<Shadow>().TwSpeed = 10 - sh.Count;
            sh.Add(go);
        }
    }

    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f, 0), Quaternion.identity);
    }

    public void JumpDust()
    {
        if (!isWall)
        {
            Instantiate(Jdust, transform.position, Quaternion.identity);
        }
    }

    void CreateWallDust()
    {
        GameObject go = Instantiate(walldust, transform.position + new Vector3(0.8f * isRight, 0, 0), Quaternion.identity);
        go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }
}
