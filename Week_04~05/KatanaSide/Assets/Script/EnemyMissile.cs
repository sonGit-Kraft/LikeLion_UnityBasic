using Unity.Cinemachine;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public float speed = 5f; // 미사일 속도
    public float lifeTime = 3f; // 미사일 생존 시간
    public int damage = 10; // 미사일 데미지
    public Vector2 direction; // 미사일 이동 방향

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    void Update()
    {
        float timeScale = TimeController.Instance.GetTimeScale();
        transform.Translate(direction * speed * Time.deltaTime * timeScale);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy")) // 적과 충돌했을 때
        {
            // 적의 죽음 애니메이션 실행
            ShootingEnemy enemy = other.GetComponent<ShootingEnemy>();
            if (enemy != null)
            {
                enemy.PlayDeathAnimation();
            }

            // 미사일 제거
            Destroy(gameObject);
        }
    }
}
