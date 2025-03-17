using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float ss = -2; // 몬스터 생성 x값 처음
    public float es = 2; // 몬스터 생성 x값 끝
    public float StartTime = 1; // 시작 시간
    public float SpawnStop = 10; // 끝나는 시간
    public GameObject monster;
    public GameObject monster2;
    public GameObject boss;

    bool Switch = true;
    bool Switch2 = true;

    [SerializeField]
    GameObject textBoosWarning;

    private void Awake()
    {
        textBoosWarning.SetActive(false);
        //PoolManager.Instance.CreatePool(monster, 10);
    }

    void Start()
    {
        StartCoroutine("RandomSpawn");
        Invoke("Stop", 10); // 10초 뒤에 Stop() 함수 실행
    }

    // 코루틴으로 랜덤 생성
    IEnumerator RandomSpawn()
    {
        while (Switch)
        {
            // 1초마다
            yield return new WaitForSeconds(StartTime);
            // x값 랜덤
            float x = Random.Range(ss, es);
            // x값은 랜덤 값 y값은 자기자신 값
            Vector2 r = new Vector2(x, transform.position.y);
            // 몬스터 생성
            Instantiate(monster, r, Quaternion.identity);
            //GameObject enemy = PoolManager.Instance.Get(monster);
            //enemy.transform.position = r;
        }
    }

    // 코루틴으로 랜덤 생성
    IEnumerator RandomSpawn2()
    {
        while (Switch2)
        {
            // 1초마다
            yield return new WaitForSeconds(StartTime + 2);
            // x값 랜덤
            float x = Random.Range(ss, es);
            // x값은 랜덤 값 y값은 자기자신 값
            Vector2 r = new Vector2(x, transform.position.y);
            //몬스터 생성
            Instantiate(monster2, r, Quaternion.identity);
        }
    }

    void Stop()
    {
        Switch = false;
        StopCoroutine("RandomSpawn");
        // 두번째 몬스터 코루틴
        StartCoroutine("RandomSpawn2");

        Invoke("Stop2", 20 + SpawnStop); // 30초 뒤에 Stop() 함수 실행
    }

    void Stop2()
    {
        Switch2 = false;
        StopCoroutine("RandomSpawn2");

        textBoosWarning.SetActive(true);

        // 보스 몬스터
        Vector3 pos = new Vector3(0, 2.97f, 0);
        Instantiate(boss, pos, Quaternion.identity);
    }
}