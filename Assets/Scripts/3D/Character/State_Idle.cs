using UnityEngine;

public class State_Idle : CharacterState_3D
{
    public State_Idle(StateMachine_3D stateMachine) : base(stateMachine)
    {
        
    }

    public override void EnterState()
    {
        stateMachine.m_currentState = CURRENT_STATE_3D.IDLE;
    }

    public override void HandleInput()
    {
        if (stateMachine.MovementInput.sqrMagnitude > .001f)
            stateMachine.ChangeState(new State_Move(stateMachine));

        if (stateMachine.HidingInput && CheckHidingPosition())
            stateMachine.ChangeState(new State_Hiding(stateMachine));
    }
}