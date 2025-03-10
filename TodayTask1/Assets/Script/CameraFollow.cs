using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 공 (Rigidbody)
    public Vector3 offset; // 카메라와 공 사이의 오프셋 (회전 방지)

    void Start()
    {
        // 카메라의 초기 오프셋을 설정
        offset = transform.position - target.position; // 현재 카메라와 공 사이의 거리 계산
    }

    void Update()
    {
        // 카메라 위치 계산 (회전과 높이를 유지한 채 공을 따라가도록)
        Vector3 targetPosition = target.position + offset; // 현재 공 위치에서 offset만큼 거리 유지

        // 카메라 위치 설정
        transform.position = targetPosition;

        // 카메라가 회전하지 않도록 (회전 고정)
        // update에서 사용하여 계속 X축 기준으로 25도 회전된 상태가 유지
        transform.rotation = Quaternion.Euler(25.0f, 0.0f, 0.0f);
    }
}