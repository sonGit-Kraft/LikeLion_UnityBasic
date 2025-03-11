using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 움직일 속도 지정
    public float moveSpeed = 2f;

    void Update()
    {
        // 움직일 거리 계산
        float distanceY = moveSpeed * Time.deltaTime;

        // 움직임 반영
        transform.Translate(0, -distanceY, 0); // 수직(Y방향)으로 움직임 
    }

    // 화면 밖으로 나가 카메라에서 보이지 않을 때 호출
    // 유니티에서의 Scene도 카메라이기 때문에 카메라와 Scene을 모두 벗어나야 없어짐
    // 따라서 Scene 창을 끄거나 카메라에 맞게 Scene 크기를 조정해야됨
    private void OnBecameInvisible()
    {
        Destroy(gameObject); // 객체 삭제   
    }
}