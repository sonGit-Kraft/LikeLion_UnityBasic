using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;             // Photon 멀티플레이어 네트워크 기능 사용
using Photon.Realtime;        // Photon 실시간 네트워크 기능 사용
using UnityEngine.UI;         // UI 요소 제어를 위해 필요
using Cinemachine;            // 시네머신 카메라 제어를 위해 필요

// 플레이어 컨트롤러 클래스: Photon 기능과 인터페이스 구현
public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    // 컴포넌트 참조
    public Rigidbody2D RB;            // 물리 효과를 위한 리지드바디
    public Animator AN;               // 애니메이션 제어를 위한 애니메이터
    public SpriteRenderer SR;         // 스프라이트 렌더링 및 방향 전환을 위함
    public PhotonView PV;             // Photon 네트워크 뷰 (소유권 확인용)
    public Text NickNameText;         // 플레이어 닉네임 표시
    public Image HealthImage;         // 체력바 UI

    // 상태 변수
    bool isGround;                    // 플레이어 접지 상태
    Vector3 curPos;                   // 현재 위치 (네트워크 동기화용)

    void Awake()
    {
        // 닉네임 설정 
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;  // 로컬 플레이어면 자신의 닉네임, 아니면 소유자의 닉네임
        NickNameText.color = PV.IsMine ? Color.green : Color.red;   // 로컬 플레이어는 녹색, 다른 플레이어는 빨간색

        if (PV.IsMine)  // 로컬 플레이어인 경우에만 카메라 설정
        {
            // 2D 카메라 설정 - 시네머신 가상카메라 찾아서 플레이어 따라가게 설정
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;    // 카메라가 플레이어를 따라감
            CM.LookAt = transform;    // 카메라가 플레이어를 주시함
        }
    }

    void Update()
    {
        if (PV.IsMine)  // 로컬 플레이어인 경우에만 제어 가능
        {
            // ← → 좌우 이동 처리
            float axis = Input.GetAxisRaw("Horizontal");  // 좌우 입력값 (-1, 0, 1)
            RB.linearVelocity = new Vector2(4 * axis, RB.linearVelocity.y);  // 속도 적용 (수평 이동)

            if (axis != 0)  // 이동 중일 때
            {
                AN.SetBool("walk", true);  // 걷기 애니메이션 활성화
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);  // 모든 클라이언트에게 방향 전환 동기화
            }
            else AN.SetBool("walk", false);  // 멈춤 상태면 걷기 애니메이션 비활성화

            // ↑ 점프, 바닥 체크
            // 플레이어 발 밑에 원형 충돌 검사로 땅 체크
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            AN.SetBool("jump", !isGround);  // 땅에 없으면 점프 애니메이션
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGround) PV.RPC("JumpRPC", RpcTarget.All);  // 위 방향키 누르고 땅에 있으면 점프

            // 스페이스 총알 발사
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 총알 생성 후 방향 설정 (플레이어 방향에 따라 오른쪽/왼쪽으로)
                PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(SR.flipX ? -0.4f : 0.4f, -0.11f, 0), Quaternion.identity)
                    .GetComponent<PhotonView>().RPC("DirRPC", RpcTarget.All, SR.flipX ? -1 : 1);
                AN.SetTrigger("shot");  // 발사 애니메이션 재생
            }
        }
        // 다른 플레이어(원격) 위치 부드럽게 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;  // 거리가 너무 멀면 즉시 이동
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);  // 아니면 부드럽게 보간
    }

    [PunRPC]
    void FlipXRPC(float axis) => SR.flipX = axis == -1;  // 스프라이트 좌우 반전 (왼쪽 이동시 flipX = true)

    [PunRPC]
    void JumpRPC()
    {
        RB.linearVelocity = Vector2.zero;  // 기존 속도 초기화
        RB.AddForce(Vector2.up * 700);     // 위쪽으로 힘 가함
    }

    public void Hit()  // 피격 처리 함수
    {
        HealthImage.fillAmount -= 0.1f;  // 체력 10% 감소
        if (HealthImage.fillAmount <= 0)  // 체력이 0 이하면 사망
        {
            GameObject.Find("Canvas").transform.Find("RespawnPanel").gameObject.SetActive(true);  // 리스폰 UI 활성화
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);  // 모든 클라이언트에게 캐릭터 제거 요청
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);  // 게임 오브젝트 파괴

    // 네트워크 동기화 처리 (IPunObservable 인터페이스 구현)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)  // 데이터 송신
        {
            stream.SendNext(transform.position);  // 위치 전송
            stream.SendNext(HealthImage.fillAmount);  // 체력 전송
        }
        else  // 데이터 수신
        {
            curPos = (Vector3)stream.ReceiveNext();  // 위치 업데이트
            HealthImage.fillAmount = (float)stream.ReceiveNext();  // 체력 업데이트
        }
    }
}