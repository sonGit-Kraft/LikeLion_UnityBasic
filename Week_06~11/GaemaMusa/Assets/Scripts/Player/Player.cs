using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Detail")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Auto Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lasttimeAttacked;
    [SerializeField] protected LayerMask whatIsEnemy;

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    public bool isBusy { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerPrimaryAttack primaryattackState { get; private set; }
    public PlayerCounterAttackState counterattackState { get; private set; }
    public PlayerAimSwordState aimswordState { get; private set; }
    public PlayerCatchSwordState catchswordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryattackState = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        counterattackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimswordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchswordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.H))
        {
            skill.crystal.CanUseSkill();
        }
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void ClearTheSword()
    {
        stateMachine.ChangeState(catchswordState);
        Destroy(sword);
    }

    public void ExitBlackHoleAbillity()
    {
        stateMachine.ChangeState(airState);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsEnemyDetected() => Physics2D.Raycast(wallCheck.position,
     Vector2.right * facingDir, 50, whatIsEnemy);

    public virtual RaycastHit2D IsEnemyDetectedLeft() => Physics2D.Raycast(wallCheck.position,
        Vector2.right * -facingDir, 50, whatIsEnemy);


    public void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if (dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;

            //dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
