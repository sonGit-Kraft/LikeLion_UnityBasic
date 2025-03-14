using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // 스피드
    public float Speed = 4.0f;
    // 공격력

    // 이펙트
    public GameObject effect;

    void Update()
    {
        // 미사일 위쪽 방향 이동
        // 위 방향 * 스피드 * 타임
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    // 화면 밖으로 나갈 경우
    private void OnBecameInvisible()
    {
        // 자기 자신 지우기
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            // 이펙트 생성
            GameObject obj = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(obj, 1);// 1초 뒤에 지우기

            // 몬스터 삭제
            // 몬스터 클래스의 함수 호출
            collision.gameObject.GetComponent<Monster>().Damage(1);

            Destroy(gameObject); // 미사일 삭제
        }

        if (collision.CompareTag("Boss"))
        {
            // 이펙트 생성
            GameObject obj = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(obj, 1);// 1초 뒤에 지우기

            // 몬스터 삭제
            // 몬스터 클래스의 함수 호출
            // collision.gameObject.GetComponent<Monster>().Damage(1);

            Destroy(gameObject); // 미사일 삭제
        }
    }
}