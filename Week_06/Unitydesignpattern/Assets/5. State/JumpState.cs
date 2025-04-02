using UnityEngine;

public class JumpState : IState
{
    public void Enter()
    {
        Debug.Log("Jump 상태 시작");
    }

    public void Update()
    {
        Debug.Log("Jump 상태 유지 중");
    }

    public void Exit()
    {
        Debug.Log("Jump 상태 종료");
    }
}
