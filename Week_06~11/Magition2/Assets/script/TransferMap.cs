using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    public Transform maptr2;
    public Transform playertr;

    public GameObject cine;
    public PolygonCollider2D range2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cine.GetComponent<CinemachineConfiner>().m_BoundingShape2D = null;
            Debug.Log("충돌들어옴");
            playertr.position = maptr2.position;
            cine.GetComponent<CinemachineVirtualCamera>().Follow = playertr;
            cine.GetComponent<CinemachineVirtualCamera>().LookAt = playertr;
            cine.GetComponent<CinemachineConfiner>().m_BoundingShape2D = range2;
        }
    }






}
