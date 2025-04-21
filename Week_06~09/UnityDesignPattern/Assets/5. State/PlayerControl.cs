using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private StateMachine stateMachine;

    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState()); // ó������ Idle ����
    }

    void Update()
    {
        stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(new JumpState()); // �����̽��ٸ� ������ ���� ���·� ����
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            stateMachine.ChangeState(new RunState()); // ����Ű�� ������ ������ �� ���·� ����
        else if (!Input.anyKey)
            stateMachine.ChangeState(new IdleState());
    }
}
