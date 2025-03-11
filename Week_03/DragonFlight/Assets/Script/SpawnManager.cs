using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnManager : MonoBehaviour
{
    // 몬스터 가져오기
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject currentEnemy; // 현재 스폰할 적
    public float spawntime = 0.5f;
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
        InvokeRepeating("SpawnEnemy", 1, spawntime);
    }

    void Update()
    {
        // 일정 점수마다 적 변경
        if (GameManager.instance.score >= 50 && GameManager.instance.score < 100 && currentEnemy != enemy2)
        {
            currentEnemy = enemy2;
            Debug.Log("적이 enemy2로 변경됨!");
            spawntime = 3f;
        }
        else if (GameManager.instance.score >= 100 && currentEnemy != enemy3)
        {
            currentEnemy = enemy3;
            Debug.Log("적이 enemy3로 변경됨!");
            spawntime = 4f;
        }
    }
}