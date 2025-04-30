using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;             // Photon 멀티플레이어 네트워크 기능을 사용하기 위한 라이브러리
using Photon.Realtime;        // Photon 실시간 네트워크 기능을 위한 라이브러리
using UnityEngine.UI;         // UI 요소를 제어하기 위한 라이브러리

// 네트워크 관리자 클래스: 포톤 네트워크 연결 및 플레이어 관리
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput;    // 사용자 닉네임 입력 필드
    public GameObject DisconnectPanel;  // 연결 해제 시 표시할 패널
    public GameObject RespawnPanel;     // 리스폰 시 표시할 패널

    void Awake()
    {
        Screen.SetResolution(960, 540, false);   // 게임 해상도 설정
        PhotonNetwork.SendRate = 60;             // 초당 데이터 전송 횟수 설정
        PhotonNetwork.SerializationRate = 30;    // 초당 상태 동기화 횟수 설정
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();  // 포톤 서버에 연결 요청

    public override void OnConnectedToMaster()
    {
        // 마스터 서버 연결 성공 시 호출
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;  // 입력한 닉네임 설정
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);  // 최대 6명 참가 가능한 방 생성 또는 참가
    }

    public override void OnJoinedRoom()
    {
        // 방 참가 성공 시 호출
        DisconnectPanel.SetActive(false);        // 연결 패널 비활성화
        StartCoroutine("DestroyBullet");         // 기존 총알 제거 코루틴 시작
        Spawn();                                 // 플레이어 캐릭터 소환
    }

    IEnumerator DestroyBullet()
    {
        // 게임 시작 시 남아있는 총알 제거하는 코루틴
        yield return new WaitForSeconds(0.2f);   // 0.2초 대기
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Bullet")) GO.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);  // 모든 총알 객체 제거
    }

    public void Spawn()
    {
        // 플레이어 캐릭터 생성
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6f, 19f), 4, 0), Quaternion.identity);  // 랜덤 위치에 플레이어 생성
        RespawnPanel.SetActive(false);  // 리스폰 패널 비활성화
    }

    void Update()
    {
        // ESC 키를 누르면 연결 해제
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 해제 시 호출
        DisconnectPanel.SetActive(true);   // 연결 해제 패널 활성화
        RespawnPanel.SetActive(false);     // 리스폰 패널 비활성화
    }
}