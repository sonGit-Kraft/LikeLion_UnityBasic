using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 공 (Rigidbody)
    public float distance = 10f; // 카메라와 공 사이의 거리
    public float height = 5f; // 카메라의 높이
    public Vector3 offset; // 카메라와 공 사이의 오프셋 (회전 방지)

    void Start()
    {
        // 카메라의 초기 오프셋을 설정
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // 카메라 위치 계산 (회전과 높이를 유지한 채 공을 따라가도록)
        Vector3 targetPosition = target.position + offset;

        // 카메라 위치 설정
        transform.position = targetPosition;

        // 카메라가 회전하지 않도록 (회전 고정)
        transform.rotation = Quaternion.Euler(30f, 0f, 0f); // 예시: 카메라는 X축 30도, Y축 0도 고정
    }
}