using System.Collections; // for Coroutine
using UnityEngine;

public class CoroutineStudy : MonoBehaviour
{
    /*
    코루틴(Coroutine)은 Unity에서 시간이 걸리는 작업을 효율적으로 실행할 수 있도록 도와주는 기능
    일반 함수와 달리 코루틴은 실행을 멈췄다가 이후 다시 실행할 수 있음
    */
    void Start()
    {
        StartCoroutine("ExampleCoroutine");
        StartCoroutine(ExampleCoroutine());
    }
    
    IEnumerator ExampleCoroutine()
    {
        Debug.Log("코루틴 시작");
        yield return new WaitForSeconds(2f); // 2초 대기 (프로그램 멈추는거 아님)
        Debug.Log("2초 후 실행");

        while (true)
        {
            Debug.Log("1초마다 실행");
            yield return new WaitForSeconds(1f);
        }
    }
}