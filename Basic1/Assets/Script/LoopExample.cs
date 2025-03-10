using UnityEngine;

public class LoopExample : MonoBehaviour
{
    void Start()
    {
        // for문: 1부터 10까지 출력
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log("Count: " + i);
        }

        // while문: 조건이 참일 때 실행
        int count = 0;
        while (count < 5)
        {
            Debug.Log("While Count: " + count);
            count++;
        }
    }
}
