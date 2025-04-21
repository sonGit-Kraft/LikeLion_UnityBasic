using UnityEngine;
using UnityEngine.Rendering;

public class Player : Entity
{
    [Header("Move Info")]
    [SerializeField] private float moveSpeed; // �̵� �ӵ�
    [SerializeField] private float jumpForce; // ���� ��

    private float xInput; // ���� �Է� ��
    
    [Header("Dash Info")]
    [SerializeField] private float dashSpeed; // �뽬 �ӵ�
    [SerializeField] private float dashDuration; // �뽬 ���� �ð�
    [SerializeField] private float dashTime; // ���� �뽬 �ð�
    [SerializeField] private float dashCooldown; // �뽬 ���� ��� �ð�
    [SerializeField] private float dashCooldownTimer; // ���� �뽬 ��ٿ� �ð�

    [Header("Attack Info")]
    [SerializeField] private float comboTime = 0.3f; // �޺� ���� �ð�
    private float comboTimeCounter; // �޺� �ð� ī����
    private bool isAttacking; // ���� ������ ����
    private int comboCounter; // �޺� Ƚ��

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        CheckInput(); // �Է� ó��
        Movement(); // �̵� ó��

        // �ð� ���� ó��
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeCounter -= Time.deltaTime;

        FlipController(); // ���� ��ȯ ó��
        AnimatorController(); // �ִϸ��̼� ó��
    }

    // ������ ������ �� ȣ��Ǵ� �Լ�
    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2) // �ִ� �޺� Ƚ���� ������ �ʱ�ȭ
            comboCounter = 0;
    }


    // �̵� ó�� �Լ�
    private void Movement()
    {
        if (isAttacking)
            rb.linearVelocity = new Vector2(0, 0); // ���� ���� �� �������� ����
        else if (dashTime > 0)
            rb.linearVelocity = new Vector2(facingDir * dashSpeed, 0); // �뽬 ���� �� y�� ����
        else
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y); // �Ϲ� �̵�
    }

    // ���� ó�� �Լ�
    private void Jump()
    {
        if (isGrounded) // �ٴڿ� ���� ���� ���� ����
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Ű �Է��� �����ϴ� �Լ�
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // �¿� �Է� �� ����

        if (Input.GetKeyDown(KeyCode.Mouse0)) // ���콺 ���� ��ư Ŭ�� �� ����
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space)) // �����̽��� �Է� �� ����
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) // ���� Shift �Է� �� �뽬
        {
            DashAbility();
        }
    }

    // ���� ���� ó�� �Լ�
    private void StartAttackEvent()
    {
        if (!isGrounded) // ���߿��� ���� �Ұ���
            return;

        if (comboTimeCounter < 0) // �޺� �ð��� ������ �ʱ�ȭ
            comboCounter = 0;

        isAttacking = true; // ���� ���·� ����
        comboTimeCounter = comboTime; // �޺� �ð� �ʱ�ȭ
    }

    // �뽬 ó�� �Լ�
    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking) // ���� ���� �ƴ� ���� �뽬 ����
        {
            dashCooldownTimer = dashCooldown; // �뽬 ��ٿ� �ʱ�ȭ
            dashTime = dashDuration; // �뽬 ���� �ð� ����
        }
    }

    // �ִϸ��̼� ���� ������Ʈ �Լ�
    private void AnimatorController()
    {
        bool isMoving = rb.linearVelocity.x != 0; // x�� �̵� ���� Ȯ��
        animator.SetFloat("yVelocity", rb.linearVelocityY); // y�� �ӵ� ����
        animator.SetBool("isMoving", isMoving); // �̵� ���� ����
        animator.SetBool("isGround", isGrounded); // �ٴ� ���� ����
        animator.SetBool("isDashing", dashTime > 0); // �뽬 ���� ����
        animator.SetBool("isAttacking", isAttacking); // ���� ���� ����
        animator.SetInteger("comboCounter", comboCounter); // �޺� Ƚ�� ����
    }

    // ���� ������ �����ϰ� ó���ϴ� �Լ�
    private void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight) // ������ �̵� ���̰� ������ ���� �ִٸ� ������
        {
            Flip();
        }
        else if (rb.linearVelocityX < 0 && facingRight) // ���� �̵� ���̰� �������� ���� �ִٸ� ������
        {
            Flip();
        }
    }
}
