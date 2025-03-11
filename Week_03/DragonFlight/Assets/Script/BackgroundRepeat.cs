using UnityEngine;

public class BackgroundRepeat : MonoBehaviour
{
    // 스크롤할 속도를 지정
    // 유니티 인스펙터에서 값을 수정했다면, 유니티 인스펙터에서 수정된 값이 유지 (코드에서 값을 초기화하거나 설정하는 부분이 없다면)
    public float scrollSpeed = 0.5f;

    //쿼드의 Material 데이터를 받아올 객체 선언
    private Material thisMaterial;

    void Start()
    {
        // 객체가 생성될 때 최초 1회 호출되는 함수
        // 현재 객체의 Component들을 참조해 Renderer라는 Component의 Material 정보 받아옴
        thisMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // 새롭게 지정해줄 Offset 객체 선언
        Vector2 newoffset = thisMaterial.mainTextureOffset; // 현재 재질에 적용된 텍스처의 오프셋(이동)을 나타내는 Vector2 값 저장

        // 스크롤 속도에 프레임을 보정해서 현재 Y값에 더해줌
        newoffset.Set(0, newoffset.y + (scrollSpeed * Time.deltaTime));

        // 최종적으로 Offset 값을 지정
        thisMaterial.mainTextureOffset = newoffset;
    }
}