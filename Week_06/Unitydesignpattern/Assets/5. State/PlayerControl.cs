using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private StateMachine stateMachine;

    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState()); // 처음에는 Idle 상태
    }

    void Update()
    {
        stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(new JumpState()); // 스페이스바를 누르면 점프 상태로 변경
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            stateMachine.ChangeState(new RunState()); // 방향키를 누르면 누르면 런 상태로 변경
        else if (!Input.anyKey)
            stateMachine.ChangeState(new IdleState());
    }
}
