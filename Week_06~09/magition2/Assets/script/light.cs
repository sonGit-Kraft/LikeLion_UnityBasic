using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    public Transform target;
    public float Speed = 2f;
    Vector3 dir;

    float angle;
    Vector3 dirNo;

    // Start is called before the first frame update
    void Start()
    {
        ////바라보는 방향벡터구하기
        //dir = target.position - transform.position;

        ////바라보는 각도구하기
        //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ////normalized 단위벡터
        //dirNo = new Vector3(dir.x, dir.y, 0).normalized;

        angle = 0;
        dirNo = new Vector3(1, 0, 0).normalized;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {

        ////회전적용
        //transform.rotation = Quaternion.Euler(0f, 0f, angle);

        ////이동적용
        //transform.position += dirNo * Speed * Time.deltaTime;


    }
}
