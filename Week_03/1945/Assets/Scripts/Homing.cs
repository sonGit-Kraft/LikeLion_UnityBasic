using Unity.VisualScripting;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public GameObject target; // 플레이어
    public float Speed = 3f;
    
    Vector2 dir;
    Vector2 dirNo;

    void Start()
    {
        // 플레이어 태그로 찾기
        target = GameObject.FindGameObjectWithTag("Player");

        // A - B: A를 바라보는 벡터 (플레이어 - 미사일)
        dir = target.transform.position - transform.position;
        // 방향 벡터(단위 벡터)만 구하기
        dirNo = dir.normalized; // 벡터를 정규화(normalize): 벡터의 길이가 1로 변경, 방향만 유지
    }

    void Update()
    {
        /* 계속 쫓아 가기 (Upadate에 넣으면 방향이 실시간으로 바뀌기 때문에)
        // A - B: A를 바라보는 벡터 (플레이어 - 미사일)
        dir = target.transform.position - transform.position;
        // 방향 벡터만 구하기 (단위 벡터)
        dirNo = dir.normalized;
        */

        transform.Translate(dirNo * Speed * Time.deltaTime);

        // 유니티 제공 함수로 구현 가능
        // transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Destroy(collision.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
} 