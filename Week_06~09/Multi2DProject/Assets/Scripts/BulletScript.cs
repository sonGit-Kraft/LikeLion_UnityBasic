using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;             // Photon 멀티플레이어 네트워크 기능 사용
using Photon.Realtime;        // Photon 실시간 네트워크 기능 사용

// 총알 스크립트 클래스: 총알의 움직임과 충돌 처리를 담당
public class BulletScript : MonoBehaviourPunCallbacks
{
    public PhotonView PV;    // Photon 네트워크 뷰 (소유권 확인용)
    int dir;                 // 총알 이동 방향 (1: 오른쪽, -1: 왼쪽)


    void Start() => Destroy(gameObject, 3.5f);  // 총알 생성 후 3.5초 뒤에 자동 제거

    void Update() => transform.Translate(Vector3.right * 7 * Time.deltaTime * dir);  // 지정된 방향으로 총알 이동


    void OnTriggerEnter2D(Collider2D col) // col을 RPC의 매개변수로 넘겨줄 수 없다
    {
        if (col.tag == "Ground") PV.RPC("DestroyRPC", RpcTarget.AllBuffered);  // 땅에 닿으면 총알 제거
        if (!PV.IsMine && col.tag == "Player" && col.GetComponent<PhotonView>().IsMine) // 느린쪽에 맞춰서 Hit판정
        {
            col.GetComponent<PlayerScript>().Hit();  // 플레이어 피격 함수 호출
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);  // 모든 클라이언트에서 총알 제거
        }
    }


    [PunRPC]
    void DirRPC(int dir) => this.dir = dir;  // 총알 방향 설정 RPC 함수

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);  // 총알 파괴 RPC 함수
}