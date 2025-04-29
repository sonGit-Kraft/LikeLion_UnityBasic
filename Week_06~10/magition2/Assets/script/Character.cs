using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float speed=3;
    protected Vector2 direction;

    private Animator myAnimator;
    private Rigidbody2D myrigid2D;

    bool isAttacking = false;
    Coroutine attackRoutine;


    //체력
    public Image hp;

    public float CurrentHp = 100;
    float PlayerMaxHp = 100;

    //기술
    public GameObject[] swordslash;
    public GameObject[] DashAttack;

    //시간
    float delay ;

    //조이스틱
    public JoyStick joystick;
    public float MoveSpeed;

    private Vector3 _moveVector;
    private Transform _transform;


    //대쉬
    public float maxSpeed;
    public float defaultSpeed = 3;
    public float dashSpeed;
    public float defaultTime;
    private float dashTime;
    private bool isdash;


    public enum Direction
    {
        
        Up =0,
        Down=1,
        Left = 2,
        Right =3
        
    }

    public Direction dir = Direction.Up;

    public enum LayerName
    {
        IdleLayer = 0,
        WalkLayer = 1,
        AttackLayer = 2
    }

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    void Start()
    {
        myrigid2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        direction = Vector2.zero;
        delay = Time.time;
        _transform = transform;
        _moveVector = Vector3.zero;
    }

    public void HandleInput()
    {
        Vector2 moveVector;
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();

        moveVector = new Vector3(h, v, 0).normalized;

        if (moveVector.x < 0 && moveVector.y == 0)
        {
            dir = Direction.Left;
        }
        else if (moveVector.x > 0 && moveVector.y == 0)
        {
            dir = Direction.Right;
        }
        else if (moveVector.y > 0 && moveVector.x == 0)
        {
            dir = Direction.Up;
        }
        else if (moveVector.y < 0 && moveVector.x == 0)
        {
            dir = Direction.Down;
        }


        direction = moveVector;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //  if (!isAttacking)
            // {
            isdash = true;
            attackRoutine = StartCoroutine(Attack());
            
            //  Skill();
            // }
        }


    }


    void GetInput()
    {
        Vector2 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        if (moveVector.x < 0 && moveVector.y == 0)
        {
            dir = Direction.Left;
        }
        else if (moveVector.x > 0 && moveVector.y == 0)
        {
            dir = Direction.Right;
        }
        else if (moveVector.y > 0 && moveVector.x == 0)
        {
            dir = Direction.Up;
        }
        else if (moveVector.y < 0 && moveVector.x == 0)
        {
            dir = Direction.Down;
        }



        direction = moveVector;

        if(Input.GetMouseButtonDown(0))
        {
            //if(!isAttacking)
            //{
            isdash = true;
            attackRoutine = StartCoroutine(Attack());

         
            // Skill();
            //}
        }

    }
    void Skill()
    {
        
        if(Time.time >= delay + 0.5f )
        {
            delay = Time.time;

            int id = (int)dir;
            switch(id)
            {
                case 0:
                    //Instantiate(swordslash[id], transform.position, Quaternion.identity);

                    break;
                case 1:
                   // Instantiate(swordslash[id], transform.position, Quaternion.identity);
                    break;
                case 2:
                    //Instantiate(swordslash[id], transform.position, Quaternion.identity);
                    break;
                case 3:
                   // Instantiate(swordslash[id], transform.position, Quaternion.identity);

                    break;
            }

         



        }

    }

    IEnumerator Attack()
    {
        isAttacking = true;
        myAnimator.SetBool("attack", isAttacking);
        
        yield return new WaitForSeconds(0.3f);
        StopAttack();
    }

    public void StopAttack()
    {
        if(attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            myAnimator.SetBool("attack", isAttacking);
         
        }
    }

    public void AttackInUp()
    {
        if (DashAttack[0] != null)
            DashAttack[0].SetActive(true);
    }

    public void AttackOutUp()
    {
        if (DashAttack[0]!=null)
        DashAttack[0].SetActive(false);
       
    }

    public void AttackInDown()
    {
        if (DashAttack[1] != null)
            DashAttack[1].SetActive(true);
    }

    public void AttackOutDown()
    {
        if (DashAttack[1] != null)
            DashAttack[1].SetActive(false);
       
    }

    public void AttackInLeft()
    {
        if (DashAttack[2] != null)
            DashAttack[2].SetActive(true);
    }

    public void AttackOutLeft()
    {
        if (DashAttack[2] != null)
            DashAttack[2].SetActive(false);
        
    }
    public void AttackInRight()
    {
        if (DashAttack[3] != null)
            DashAttack[3].SetActive(true);
    }

    public void AttackOutRight()
    {
        if (DashAttack[3] != null)
            DashAttack[3].SetActive(false);
    }




    // Update is called once per frame
    void Update()
    {
        GetInput();
        //HandleInput();
        HandleLayers();
       
    }
   



    public void Angle(float angle)
    {
        // 45     135     -45    -135
        if (angle > -45 && angle <= 45)
        {
            //x
            myAnimator.SetFloat("x", 1);
            myAnimator.SetFloat("y", 0);
        }
        else if (angle > 45 && angle <= 135)
        {
            //y
            myAnimator.SetFloat("x", 0);
            myAnimator.SetFloat("y", 1);
        }
        else if (angle > 135 && angle <= 180 || angle <= -135)
        {
            //-x
            myAnimator.SetFloat("x", -1);
            myAnimator.SetFloat("y", 0);
        }
        else if (angle > -135 && angle <= -45)
        {
            //-y
            myAnimator.SetFloat("x", 0);
            myAnimator.SetFloat("y", -1);
        }
    }



    public void HandleLayers()
    { 
        if(IsMoving)
        {
            ActivateLayer(LayerName.WalkLayer);
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);
        }
        else if(isAttacking)
        {
            ActivateLayer(LayerName.AttackLayer);
        }
        else
        {
            ActivateLayer(LayerName.IdleLayer);
        }    
    }

    public void ActivateLayer(LayerName layerName)
    {
        for(int i =0; i<myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(1, 0);
        }
        myAnimator.SetLayerWeight((int)layerName, 1);
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        //myrigid2D.velocity = direction.normalized * speed;
        myrigid2D.velocity = direction.normalized * defaultSpeed;

        

        if (dashTime <= 0)
        {
            defaultSpeed = maxSpeed;
            if (isdash)
                dashTime = defaultTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
            defaultSpeed = dashSpeed;

        }

        isdash = false;


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("트리거충돌");
        if (collision.tag == "Bullet")
        {
            float Damage = 1;
            if(CurrentHp > 0)
            {
                CurrentHp -= Damage;
                hp.fillAmount = CurrentHp / PlayerMaxHp;
               
            }
           


            Destroy(collision.gameObject);
        }

        if(collision.tag == "monster")
        {
            float Damage = 1;
            if (CurrentHp > 0)
            {
                CurrentHp -= Damage;
                hp.fillAmount = CurrentHp / PlayerMaxHp;

            }
        }
    }

    //아이템먹으면Hp오르는 함수
    public void ItemEffHp(int ItemVal)
    {
        CurrentHp += ItemVal;
        hp.fillAmount = CurrentHp / PlayerMaxHp;
    }


}
