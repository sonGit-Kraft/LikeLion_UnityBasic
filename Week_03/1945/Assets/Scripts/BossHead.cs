using UnityEngine;

public class BossHead : MonoBehaviour
{
    [SerializeField] // 직렬화
    GameObject Bossbullet; 

    // 애니메이션에서 함수 사용
    public void RightDownLaunch()
    {
        GameObject go = Instantiate(Bossbullet, transform.position, Quaternion.identity);

        go.GetComponent<BossBullet>().Move(new Vector2(1, -1)); // 오른쪽 아래 방향
    }

    public void LeftDownLaunch()
    {
        GameObject go = Instantiate(Bossbullet, transform.position, Quaternion.identity);

        go.GetComponent<BossBullet>().Move(new Vector2(-1, -1)); // 왼쪽 아래 방향
    }

    public void DownLaunch()
    {
        GameObject go = Instantiate(Bossbullet, transform.position, Quaternion.identity);

        go.GetComponent<BossBullet>().Move(new Vector2(0, -1)); // 아래 방향
    }
}
