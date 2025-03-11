using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    public GameObject bullet; // 총알 프리팹 가져올 변수
                              // GameObject: 모든 게임 오브젝트의 기본 클래스
    void Start()
    {
        // InvokeRepeating()은 특정 메서드를 일정한 간격으로 반복 호출
        // InvokeRepeating("메서드이름", 시작시간, 반복시간);
        InvokeRepeating("Shoot", 1.5f, 5f);
    }

    void Shoot()
    {
        // 총알 프리팹, 적 런처 포지션, 방향값 안줌
        // Instantiate()는 Unity에서 게임 오브젝트 또는 프리팹을 복제(인스턴스화) 하는 함수
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}