using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animName) 
        : base(stateMachine, player, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Upadte()
    {
        base.Upadte();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
