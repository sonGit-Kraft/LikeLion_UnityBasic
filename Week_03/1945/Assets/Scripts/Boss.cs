using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    int flag = 1;
    int speed = 2;

    public GameObject mb;
    public GameObject mb2;
    public Transform pos1;
    public Transform pos2;

    void Start()
    {
        StartCoroutine(BossMissile());
        StartCoroutine(CircleFire());
    }

    IEnumerator BossMissile()
    {
        while (true)
        {
            Instantiate(mb, pos1.position, Quaternion.identity);
            Instantiate(mb, pos2.position, Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator CircleFire()
    {
        // 공격력
        float attackRate = 3;
        // 발사체 생성 개수
        int count = 30;
        // 발사체 사이의 각도
        float intervalAngle = 360 / count;
        // 가중되는 각도
        float weightAngle = 0f;

        // 원 형태로 발사하는 발사체 생성 (count 개수 만큼)
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                // 발사체 생성
                GameObject clone = Instantiate(mb2, transform.position, Quaternion.identity);

                // 발사체 이동 방향 (각도)
                float angle = weightAngle + intervalAngle * i;

                // 발사체 이동 방향 (벡터)
                // cos(각도) 라디안 단위의 각도 표현을 위해 pi/180 곱합
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                // sin(각도) 라디안 단위의 각도 표현을 위해 pi/180 곱합
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                // 발사체 이동 방향 설정
                clone.GetComponent<BossBullet>().Move(new Vector2(x, y));
            }
            // 발사체가 생성되는 시작 각도 설정
            weightAngle += 1;

            // 3초마다 미사일 발사
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void Update()
    {
        if (transform.position.x >= 1)
            flag *= -1;
        if (transform.position.x <= -1)
            flag *= -1;

        transform.Translate(flag * speed * Time.deltaTime, 0, 0);
    }
}