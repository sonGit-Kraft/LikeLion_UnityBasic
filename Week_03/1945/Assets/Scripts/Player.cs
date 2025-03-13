using UnityEngine;

public class Player : MonoBehaviour
{
    // 스피드
    public float moveSpeed = 5.0f;

    // 화면 경계를 저장할 변수
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // 애니메이터를 가져올 변수
    Animator ani;

    // 미사일
    public GameObject bullet; // 추후 4개 배열로 만들 예정
    public Transform pos = null;

    // 아이템

    // 레이저

    void Start()
    {
        ani = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져옴

        Camera cam = Camera.main;
        // Camera.main.ViewportToWorldPoint() : 카메라 뷰포트 좌표(0~1 범위)를 월드 좌표로 변환하는 함수
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)); // (0, 0, 0) → 화면의 왼쪽 아래 (bottom - left)
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0)); // (1, 1, 0) → 화면의 오른쪽 위 (top - right)

        // 화면 경계 설정
        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);
    }

    void Update()
    {
        // 방향키에 따른 움직임
        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"); // 좌우 키
        float moveY = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"); // 상하 키

        // -1 ~ 0 ~ 1
        if(Input.GetAxis("Horizontal") <= -0.25f) // 수평 방향 입력 값이 -0.25 이하일 때
            ani.SetBool("left", true);
        else
            ani.SetBool("left", false);

        if(Input.GetAxis("Horizontal") >= 0.25f) // 수평 방향 입력 값이 0.25 이상일 때
            ani.SetBool("right", true);
        else
            ani.SetBool("right", false);

        if (Input.GetAxis("Vertical") >= 0.1f) // 수직 방향 입력 값이 0.1 이상일 때
            ani.SetBool("up", true);
        else
            ani.SetBool("up", false);

        // 스페이스바 -> 미사일 발사
        if(Input.GetKeyDown(KeyCode.Space)) // 키를 한 번 눌렀을 때만 실행 (Input.GetKey: 키를 꾹 누르는 동안 실행)
        {
            // 프리팹, 위치, 방향
            Instantiate(bullet, pos.position, Quaternion.identity);
        }

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // 경계를 벗어나지 않도록 위치 제한
        // Mathf.Clamp(value, min, max): value가 min보다 작으면 min, max보다 크면 max로 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        transform.position = newPosition;
    }
}