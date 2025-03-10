using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도

    void Update()
    {
        /*
        // Vector3.right; // -> (1, 0, 0) 
        // Vector3.left;  // <- (-1, 0, 0) 
        
        // 키 입력에 따라 이동 (좌우)
        float move = Input.GetAxis("Horizontal"); // Horizontal: 좌우 키, 범위: -1 ~ 1

        // move에 따라 방향 정해짐 
        // Time.deltaTime: 매 프레임의 시간 간격을 나타내는 값 -> 프레임 수와 관계없이 일정한 속도로 움직임을 구현 (쉽게 말해 부드럽게 움직이게 해줌)
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);
        */

        // 키 입력에 따라 이동 (상하좌우)
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // Vertical: 상하 키

        // transform.position += move * speed * Time.deltaTime;
        transform.Translate(move * speed * Time.deltaTime);
    }
}