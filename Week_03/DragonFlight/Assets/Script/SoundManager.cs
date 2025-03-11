using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤
    public static SoundManager instance; // 자기 자신을 변수로 담기

    AudioSource myAudio; // AudioSource 컴포넌트를 변수로 담음
    public AudioClip soundBullet; // 총알 소리
    public AudioClip soundDie; // 죽는 소리

    private void Awake()
    {
        if(SoundManager.instance == null) // 인스터스가 있는지 검사
        {
            SoundManager.instance = this; // 자기 자신을 담음
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기    
    }

    // 총알 발사 사운드
    public void PlayBulletSound()
    {
        myAudio.PlayOneShot(soundBullet); // PlayOneShot: 한번만 실행
    }

    // 몬스터 죽는 사운드
    public void PlayDieSound()
    {
        myAudio.PlayOneShot(soundDie); // PlayOneShot: 한번만 실행
    }
}
