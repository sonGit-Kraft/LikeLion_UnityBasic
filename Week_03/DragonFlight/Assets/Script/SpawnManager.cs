using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnManager : MonoBehaviour
{
    // 몬스터 가져오기
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject currentEnemy; // 현재 스폰할 적

    public float spawnTime = 0.5f; // 스폰 주기

    // 적을 생성하는 함수
    void SpawnEnemy()
    {
        float randomX = Random.Range(-2f, 2f); // 적이 나타날 랜덤 X좌표 생성 (범위: -2 ~ 2)

        // 적을 생성 
        Instantiate(currentEnemy, new Vector3(randomX, transform.position.y, 0f), Quaternion.identity);
    }

    void Start()
    {
        // SpawnEnemy 1 0.5f
        InvokeRepeating("SpawnEnemy", 1, spawnTime);
    }

    void Update()
    {
        // 일정 점수마다 적 변경
        if (GameManager.instance.score >= 50 && GameManager.instance.score < 100 && currentEnemy != enemy2)
        {
            if (spawnTime != 0.75f)  // spawnTime이 실제로 변경될 때만
            {
                currentEnemy = enemy2;
                spawnTime = 0.75f;
                CancelInvoke("SpawnEnemy");  // 기존 호출 취소
                InvokeRepeating("SpawnEnemy", 1, spawnTime);  // 새로운 주기로 호출
            }
        }
        else if (GameManager.instance.score >= 100 && currentEnemy != enemy3)
        {
            if (spawnTime != 1.0f)  // spawnTime이 실제로 변경될 때만
            {
                currentEnemy = enemy3;
                spawnTime = 1.0f;
                CancelInvoke("SpawnEnemy");  // 기존 호출 취소
                InvokeRepeating("SpawnEnemy", 1, spawnTime);  // 새로운 주기로 호출
            }
        }
    }
}