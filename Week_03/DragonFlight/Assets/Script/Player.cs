using UnityEngine;

public class Player : MonoBehaviour
{
    // 움직이는 속도 정의
    public float moveSpeed = 5.0f;
    public GameObject playerHit;

    /*
    Update()
    - 프레임 단위로 실행된다.즉, 게임의 프레임 속도(FPS)에 따라 실행 횟수가 달라진다.
    - 일반적인 로직(입력 처리, UI 업데이트, 애니메이션 등)에 사용된다.
    - 프레임마다 실행되므로 불규칙적인 실행 간격을 가질 수 있다.

    FixedUpdate()
    - 고정된 시간 간격(기본값: 0.02초)**마다 실행된다. (물리 엔진과 동기화됨)
    - Unity의 물리 연산(Physics) 관련 작업을 처리하는 데 사용된다.
    - Time.fixedDeltaTime 간격으로 실행되므로, 일정한 시간 간격을 유지한다.

    일반적인 로직 (플레이어 입력, 애니메이션 등) → Update()
    물리 연산 (Rigidbody 이동, 충돌 감지 등) → FixedUpdate()
    */

    void FixedUpdate() // FixedUpdate로 하면 벽 충돌 시 떨리지 않음
    {
        moveControl();
    }

    void moveControl()
    {
        // 입력된 방향(Axis)을 확인하고 속도와 시간 값을 곱해 이동할 거리 계산
        float distanceX = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed; // Horizontal: 좌우 키, FixedUpdate이기 때문에 Time.fixedDeltaTime 사용 
        // 이동량만큼 실제로 이동시켜주는 함수
        transform.Translate(distanceX, 0, 0); // 좌우만 이동
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 적이 부딪힘
        // if(other.gameObject.tag == "Enemy") // 충돌한 object의 태그가 "Enemy" 일 때
        if (collision.gameObject.CompareTag("Enemy")) // CompareTag -> 좀 더 안정적으로 비교
        {
            GameManager.instance.DecreaseHp(10); // Hp 감소
            Instantiate(playerHit, transform.position, Quaternion.identity); // 히트 이펙트 생성

            if (GameManager.instance.hp <= 0)
            {
                Destroy(gameObject, 0.5f); // 0.5초 후 플레이어 삭제
                Invoke("QuitGame", 0.5f); // 0.5초 후 게임 종료
            }
        }
    }
}