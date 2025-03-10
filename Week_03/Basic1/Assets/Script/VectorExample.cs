using UnityEngine;

public class VectorExample : MonoBehaviour
{
    // public Vector2 v2 = new Vector2(10, 10); // (x, y)
    // public Vector3 v3 = new Vector3(1, 1, 1); // (x, y, z)
    
    void Start()
    {
        // 벡터의 더하기
        Vector3 a = new Vector3(1, 1, 0);
        Vector3 b = new Vector3(2, 0, 0);

        Vector3 c = a + b; // (3, 1, 0)

        Debug.Log("Vector " + c);
        Debug.Log("길이: " + c.magnitude); // 벡터의 길이 (3, 1, 0) -> sqrt(3^2 + 1^2) = 길이
                                           // 벡터 계산 참고 자료: https://wergia.tistory.com/209
        // 정규화 normalize
        // 벡터의 크기를 1로 만들고 방향만 유지
        Vector3 d = new Vector3(3, 0, 0);
        Vector3 normalizedVector = d.normalized;

        Debug.Log("1의 길이 방향만 설정하는 정규화: " + normalizedVector);

        // 벡터의 빼기
        Vector3 e = a - b; // (3, 1, 0)
    }
    

    void Update()
    {
        
    }
}
