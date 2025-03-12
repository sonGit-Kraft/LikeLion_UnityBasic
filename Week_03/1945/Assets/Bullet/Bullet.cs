using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    //public GameObject explosion;

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

    /*
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
    */
}
