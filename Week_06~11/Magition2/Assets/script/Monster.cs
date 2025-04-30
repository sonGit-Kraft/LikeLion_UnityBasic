using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster: MonoBehaviour
{
    bool damaged = false;
    SpriteRenderer mst;
    public float flashSpeed = 1f;
    Color origin;

    public GameObject fire;
    public Transform player;
    float delayTime = 0;

    Animator monAny;
    float angle;
    Vector3 dir;


    //체력
    public Image Monhp;

    float CurrentHp = 50;
    float MonMaxHp = 50;

    //넉백
    bool isknockback= false;
    public float speed = 2f;


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
        if (dis < 20f)
        {
            if (Time.time >= delayTime + 2)
            {
                delayTime = Time.time;
                GameObject gFire = Instantiate(fire, transform.position, Quaternion.identity);
                gFire.GetComponent<fire>().target = player;
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
        if(collision.gameObject.tag == "Player")
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

            float x = transform.position.x - collision.transform.position.x;

            if (x < 0)
                x = 1;
            else
                x = -1;

            float y = transform.position.x - collision.transform.position.x;

            if (y < 0)
                y = 1;
            else
                y = -1;

            StartCoroutine(KnockbackX(x));
            StartCoroutine(KnockbackY(y));
            // Destroy(collision.gameObject);
        }
    }

    IEnumerator KnockbackX(float dir)
    {
        isknockback = true;

        float ctime = 0;

        while(ctime <0.2f)
        {
            if(transform.rotation.y ==0)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime * dir);

            }
            else
            {
                transform.Translate(Vector2.left * flashSpeed * Time.deltaTime * -1f * dir);
            }
            ctime += Time.deltaTime;
            yield return null;
        }
        isknockback = false;
    }


    IEnumerator KnockbackY(float dir)
    {
        isknockback = true;

        float ctime = 0;

        while (ctime < 0.2f)
        {
            if (transform.rotation.x == 0)
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime * dir);

            }
            else
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime * -1f * dir);
            }
            ctime += Time.deltaTime;
            yield return null;
        }
        isknockback = false;
    }


}
