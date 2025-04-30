using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun, IPunObservable
{
    public float speed;
    public Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        if (!photonView.AmOwner)
        {
            return;
        }

        Launch();
    }

    private void Launch()
    {
        if (!photonView.AmOwner)
        {
            return;
        }

        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        rigidbody.linearVelocity = new Vector2(x * speed, y * speed);
    }

    public void Reset()
    {
        rigidbody.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke("Launch", 1);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.linearVelocity);
        }
        else
        {
            rigidbody.position = (Vector2)stream.ReceiveNext();
            rigidbody.linearVelocity = (Vector2)stream.ReceiveNext();
        }
    }
}