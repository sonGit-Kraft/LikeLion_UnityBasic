using Photon.Pun;
using UnityEngine;

public class NetPaddle : MonoBehaviourPun
{
    public float speed = 10f;

    void Update()
    {
        if (photonView.IsMine) // �ڱ� �ڽ� �÷��� �ϴ� ��ü (��Ʈ��ũ�� �ڱ� �ڽ��� ��Ʈ�� �ϴ��� �ٸ� �÷��̾ ��Ʈ�� �ϴ��ĸ� ����ؾ� �Ѵ�)
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(0, move, 0);
        }
    }
}
