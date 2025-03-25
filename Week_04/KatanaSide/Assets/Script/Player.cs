using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 속성")]
    public float speed = 5;
    public float jumpUp = 1;
    public float power;
    public Vector3 direction;
    public GameObject slash;

    // 그림자
    public GameObject Shadow1;
    List<GameObject> sh = new List<GameObject>();

    // 히트 이펙트
    public GameObject hit_lazer;

    bool bJump = false;
    Animator pAnimator;
    Rigidbody2D pRig2D;
    SpriteRenderer sp;

    void Start()
    {
        pAnimator = GetComponent<Animator>();
        pRig2D = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
        sp = GetComponent<SpriteRenderer>();
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal"); // GetAxisRaw: -1 0 1

        if (direction.x < 0)
        {
            // left
            sp.flipX = true;
            pAnimator.SetBool("Run", true);

            // ShadowFlip
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].GetComponent<SpriteRenderer>().flipX = sp.flipX;
            }
        }
        else if (direction.x > 0)
        {
            // right
            sp.flipX = false;
            pAnimator.SetBool("Run", true);

            // ShadowFlip
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].GetComponent<SpriteRenderer>().flipX = sp.flipX;
            }
        }
        else if (direction.x == 0)
        {
            pAnimator.SetBool("Run", false);

            for (int i = 0; i < sh.Count; i++)
            {
                Destroy(sh[i]); // 게임오브젝트 지우기
                sh.RemoveAt(i); // 게임오브젝트 관리하는 리스트 지우기
            }
        }

        if (Input.GetMouseButtonDown(0)) // 0번 -> 왼쪽마우스
        {
            pAnimator.SetTrigger("Attack");
            Instantiate(hit_lazer, transform.position, Quaternion.identity);
        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(pRig2D.position, Vector3.down, new Color(0, 1, 0));

        // 레이캐스트로 땅 체크
        RaycastHit2D rayHit = Physics2D.Raycast(pRig2D.position, Vector2.down, 1, LayerMask.GetMask("Ground"));

        if (pRig2D.linearVelocityY < 0) // Y축 속도가 음수일 때 (내려오고 있을 때)
        {
            if (rayHit.collider != null) // 땅과 충돌한 경우
            {
                if (rayHit.distance < 0.7f) // 땅과의 거리 0.7 이하일 때
                {
                    pAnimator.SetBool("Jump", false); // 점프 애니메이션 종료
                }
            }
        }
    }

    void Update()
    {
        KeyInput();
        Move();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (pAnimator.GetBool("Jump") == false)
            {
                Jump();
                pAnimator.SetBool("Jump", true);
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
        if (sp.flipX == false) // 플레이어 오른쪽
        {
            pRig2D.AddForce(Vector2.right * power, ForceMode2D.Impulse);
            GameObject go = Instantiate(slash, transform.position, Quaternion.identity);
            go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
        else // 플레이어 왼쪽
        {
            pRig2D.AddForce(Vector2.left * power, ForceMode2D.Impulse);
            GameObject go = Instantiate(slash, transform.position, Quaternion.identity);
            go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
    }

    // 그림자
    public void RunShadow()
    {
        if (sh.Count < 6)
        {
            GameObject go = Instantiate(Shadow1, transform.position, Quaternion.identity);
            go.GetComponent<Shadow>().TwSpeed = 10 - sh.Count;
            sh.Add(go);
        }
    }
}