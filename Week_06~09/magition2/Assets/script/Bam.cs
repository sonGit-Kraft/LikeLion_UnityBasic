using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bam : MonoBehaviour
{
    bool damaged = false;
    SpriteRenderer mst;
    public float flashSpeed = 1f;
    Color origin;

    //번개
    public GameObject light;
    public int numberOfObjects = 20;
    public float radius = 0.5f;
    float speed = 100f;

    public Transform player;
    float delayTime = 0;

    Animator monAny;
    float angle;
    Vector3 dir;

    //체력
    public Image Monhp;

    float CurrentHp = 150;
    float MonMaxHp = 50;

    private void Start()
    {
        monAny = GetComponent<Animator>();
        mst = GetComponent<SpriteRenderer>();
        origin = mst.color;
        delayTime = Time.deltaTime;
    }
    private void Update()
    {
        if (damaged)
        {
            mst.color = Color.red;
        }
        else
        {
            mst.color = Color.Lerp(mst.color, origin
                , flashSpeed * Time.deltaTime);
        }
        damaged = false;

        float dis = Vector3.Distance(player.position, transform.position);

        //거리 5이상 벗어나면 파이어 안쏨
        if (dis < 10f)
        {
            if (Time.time >= delayTime + 3)
            {
                delayTime = Time.time;

            
                //360/프리팹갯수
                float angle = 360 / numberOfObjects;

                for(int i =0; i< numberOfObjects; i++)
                {
                    GameObject obj;

                    obj = (GameObject)Instantiate(light, transform.position, Quaternion.identity);

                    //생성발사
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / numberOfObjects), speed * Mathf.Sin(Mathf.PI * i * 2 / numberOfObjects)));
                    //방향
                    obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / numberOfObjects - 0));
               
                }



            }

        }

        AngleAnimation();




    }

    void AngleAnimation()
    {
        //각도에따른 애니메이션 구현
        //바라보는 방향벡터구하기
        dir = player.position - transform.position;

        //바라보는 각도구하기
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        //Debug.Log("각도 : " + angle);

        // 45     135     -45    -135
        if (angle > -45 && angle <= 45)
        {
            //x
            monAny.SetFloat("x", 1);
            monAny.SetFloat("y", 0);
        }
        else if (angle > 45 && angle <= 135)
        {
            //y
            monAny.SetFloat("x", 0);
            monAny.SetFloat("y", 1);
        }
        else if (angle > 135 && angle <= 180 || angle <= -135)
        {
            //-x
            monAny.SetFloat("x", -1);
            monAny.SetFloat("y", 0);
        }
        else if (angle > -135 && angle <= -45)
        {
            //-y
            monAny.SetFloat("x", 0);
            monAny.SetFloat("y", -1);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            damaged = true;
            //SpriteRenderer msr = collision.gameObject.GetComponent<SpriteRenderer>();
            //msr.color = Color.red;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("트리거충돌");
        if (collision.tag == "PlayerSkill")
        {
            damaged = true;
            float Damage = 10;
            if (CurrentHp > 0)
            {
                CurrentHp -= Damage;
                Monhp.fillAmount = CurrentHp / MonMaxHp;

            }
            else
            {
                ItemDatabase.instance.ItemDrop(transform.position);
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }
    }




}


