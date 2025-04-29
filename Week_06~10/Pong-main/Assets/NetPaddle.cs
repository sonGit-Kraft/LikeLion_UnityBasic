using Photon.Pun;
using UnityEngine;

public class NetPaddle : MonoBehaviourPun
{
    public float speed = 10f;

    void Update()
    {
        if (photonView.IsMine) // 자기 자신 플레이 하는 주체 (네트워크는 자기 자신을 컨트롤 하느냐 다른 플레이어를 컨트롤 하느냐를 고민해야 한다)
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(0, move, 0);
        }
    }
}
