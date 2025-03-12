using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject bullet; // 총알 프리팹 가져올 변수

    void Start()
    {
        // InvokeRepeating()은 특정 메서드를 일정한 간격으로 반복 호출
        // InvokeRepeating("메서드이름", 시작시간, 반복시간);
        InvokeRepeating("Shoot", 0.5f, 0.1f);
    }

    void Shoot()
    {
        // 총알 프리팹, 런처 포지션, 방향값 안줌
        // Instantiate()는 Unity에서 게임 오브젝트 또는 프리팹을 복제(인스턴스화) 하는 함수
        Instantiate(bullet, transform.position, Quaternion.identity);


        // 총알 발사 사운드 (사운드 매니저에서 총알 발사 사운드 실행 함수 호출 (싱글톤 사용))
        // SoundManager.instance.PlayBulletSound();
    }
}
