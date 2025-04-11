using UnityEngine;

public class StateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit(); // 이전 상태의 Exit() 실행 (?.: null이 아닐 경우에만 해당 멤버 호출)
        currentState = newState; // 새 상태로 변경
        currentState.Enter(); // 새 상태로 Enter() 실행
    }

    public void Update()
    {
        currentState?.Update(); // 현재 상태의 Update() 실행
    }
}
