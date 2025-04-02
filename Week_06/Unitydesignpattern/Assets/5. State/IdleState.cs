using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("Idle 상태 시작");
    }

    public void Update()
    {
        Debug.Log("Idle 상태 유지 중");
    }

    public void Exit()
    {
        Debug.Log("Idle 상태 종료");
    }
}
