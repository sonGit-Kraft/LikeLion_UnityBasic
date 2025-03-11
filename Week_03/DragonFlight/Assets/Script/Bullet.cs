using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 0.45f;
    public GameObject explosion;

    void Start()
    {
           
    }

    void Update()
    {
        // Y축 이동
        transform.Translate(0, moveSpeed * Time.deltaTime, 0);
    }

    // 미사일이 화면 밖으로 나가면
    private void OnBecameInvisible()
    {
        // 미사일 지우기
        Destroy(gameObject); // gameObject: 자기 자신
    }

    // 2D 물리 충돌 감지 트리거 이벤트
    // 다른 Collider2D가 트리거(Collider2D + IsTrigger 활성화) 영역에 들어왔을 때 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알과 적이 부딪힘
        // if(other.gameObject.tag == "Enemy") // 충돌한 object의 태그가 "Enemy" 일 때
        if (collision.gameObject.CompareTag("Enemy")) // CompareTag -> 좀 더 안정적으로 비교
        {                                             
            // 폭팔 프리팹, 총알 포지션, 방향값 안줌
            Instantiate(explosion, transform.position, Quaternion.identity); // 폭팔 이펙트 생성
            SoundManager.instance.PlayDieSound(); // 죽음 사운드
            GameManager.instance.AddScore(10); // 점수 올려주기
            Destroy(collision.gameObject); // 적 지우기
            Destroy(gameObject); // 총알 지우기
        }
    }

    /* Unity 충돌 처리
    1. 충돌을 위한 필요한 조건
    - 각 오브젝트에 2D 콜라이더(Collider2D)가 있어야 함
    - 둘 중 하나는 Rigidbody2D를 가져야 충돌 감지 가능

    2. 충돌 이벤트 종류

    // IsTrigger가 체크되지 않은 상태

    // 다른 Collider2D와 충돌했을 때 (시작)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + "과(와) 충돌함!");
    }

    // 충돌이 계속되는 동안 (프레임마다 실행됨)
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + "과(와) 충돌 중...");
    }

    // 충돌이 끝났을 때 (Collider에서 벗어남)
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + "과(와) 충돌 종료");
    }

    // IsTrigger가 체크된 상태 (뚫고 가는 충돌)

    // 트리거에 진입했을 때 (시작)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + "이(가) 트리거에 들어옴!");
    }

    // 트리거 내부에 머무를 때 (프레임마다 실행됨)
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + "이(가) 트리거 내부에 있음...");
    }

    // 트리거에서 벗어났을 때 (종료)
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + "이(가) 트리거에서 나감");
    }
    */
}
