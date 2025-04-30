using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Z))
            stateMachine.ChangeState(player.blackholeState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryattackState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimswordState);

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.counterattackState);

        if (!player.IsGroundDetected()) // �� ������ �ƴ� ��
            stateMachine.ChangeState(player.airState); // airState�� ����

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();

        return false;
    }
}
