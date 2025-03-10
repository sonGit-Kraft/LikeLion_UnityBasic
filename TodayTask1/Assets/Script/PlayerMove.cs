using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1.0f; // 공의 굴러가는 속도
    public float jumpForce = 5.0f; // 점프의 힘

    private Rigidbody rb; // Rigidbody 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 공을 굴리는 처리 (WASD 또는 화살표 키로 움직임)
        float horizontal = Input.GetAxis("Horizontal"); // 좌우 움직임
        float vertical = Input.GetAxis("Vertical"); // 상하 움직임

        // 공을 경사로에서 굴리기 위해 Rigidbody에 힘을 추가
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);

        // 스페이스바로 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
        }
    }
}
