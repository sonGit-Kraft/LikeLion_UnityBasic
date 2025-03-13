using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Speed = 3f;
    public float Delay = 1f;
    public Transform ms1;
    public Transform ms2;
    public GameObject bullet;

    void Start()
    {
        // Invoke: 주로 델리게이트 또는 메서드를 호출할 때 사용
        Invoke("CreateBullet", Delay);
    }

    void CreateBullet()
    {
        Instantiate(bullet, ms1.position, Quaternion.identity);
        Instantiate(bullet, ms2.position, Quaternion.identity);

        // 재귀 호출
        Invoke("CreateBullet", Delay);
        // 재귀 방식이 싫다면 Start()에서 InvokeRepeating() 쓰면 됨

    }
    void Update()
    {
        // 아래 방향으로 움직여라
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}