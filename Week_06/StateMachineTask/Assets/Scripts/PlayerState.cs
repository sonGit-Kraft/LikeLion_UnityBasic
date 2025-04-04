using UnityEditor;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animName;
    
    public PlayerState(PlayerStateMachine stateMachine, Player player, string animName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        Debug.Log("Enter: " + animName);
    }
    public virtual void Upadte()
    {
        Debug.Log("Upadte: " + animName);
    }
    public virtual void Exit()
    {
        Debug.Log("Exit: " + animName);
    }
}
