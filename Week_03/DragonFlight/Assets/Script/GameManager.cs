using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    // 어디에서나 접근 할 수 있도록 static으로 자기자신을 저장해서 싱글톤이라는 디자인 패턴 사용
    public static GameManager instance;
    public Text ScoreText; // 점수를 표시하는 Text 객체를 에디터에서 받아옴
    public Text StartText; // 게임 시작 전 3, 2, 1
    public Text HpText; // Hp를 표시하는 Text 객체를 에디터에서 받아옴
    public int score = 0; // 점수 초기화
    public int hp = 100;

    private void Awake()
    {
        if (instance == null) // 정적으로 자기 자신 체크
        {
            instance = this; // 자기 자신 저장
        }
    }

    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Time.timeScale = 0; //전체 시간멈춤

        int i = 3;
        while (i > 0)
        {
            StartText.text = i.ToString();
            // yield return new WaitForSeconds(1f); // 1초 대기
            yield return new WaitForSecondsRealtime(1); //게임이 멈춰도 동작하는 대기
            i--;

            if (i == 0)
            {
                StartText.gameObject.SetActive(false); // UI 감추기
                Time.timeScale = 1; //다시 시간 정상으로
            }
                
        }
    }

    public void AddScore(int num)
    {
        score += num; // 점수를 더함
        ScoreText.text = "Score: " + score; // 텍스트에 반영
    }

    public void Hp(int num)
    {
        hp -= num;
        HpText.text = "Hp: " + hp; // 텍스트에 반영
    }
}