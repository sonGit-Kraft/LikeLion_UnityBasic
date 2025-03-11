using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyBullet : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public GameObject explosion;

    void Update()
    {
        // Y축 이동
        transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
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
        // 충돌한 object의 태그가 "Player" 일 때
        if (collision.gameObject.CompareTag("Player")) // CompareTag -> 좀 더 안정적으로 비교
        {
            Instantiate(explosion, transform.position, Quaternion.identity); // 폭팔 이펙트 생성
            GameManager.instance.DecreaseHp(10); // Hp 감소
            Destroy(gameObject); // 총알 지우기
            if (GameManager.instance.hp <= 0)
                Invoke("QuitGame", 0.5f); // 0.5초 후 게임 종료
        }
    }
}