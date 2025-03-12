using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float scrollSpeed = 0.01f;
    Material myMaterial;

    void Start()
    {
        myMaterial = GetComponent<Renderer>().material; // 현재 게임 오브젝트의 Renderer 컴포넌트에서 material을 가져와 변수에 저장
    }

    void Update()
    {
        float newOffsetY = myMaterial.mainTextureOffset.y + scrollSpeed * Time.deltaTime;
        Vector2 newOffset = new Vector2(0, newOffsetY);

        myMaterial.mainTextureOffset = newOffset;
    }
}