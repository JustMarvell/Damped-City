using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class State_Move : CharacterState_3D
{
    public State_Move(StateMachine_3D stateMachine) : base(stateMachine) { }

    Vector2 input;
    Vector3 cameraForward, cameraRight;
    PLAYBACK_STATE pLAYBACK_STATE;

    public override void EnterState()
    {
        stateMachine.m_currentState = CURRENT_STATE_3D.MOVE;

        footsteps.getPlaybackState(out pLAYBACK_STATE);

        if (pLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
        {
            footsteps.start();
        }
    }

    public override void FixedUpdateState()
    {
        input = stateMachine.MovementInput;
        cameraForward = cam.transform.forward;
        cameraRight = cam.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 mv = (cameraForward * input.y + cameraRight * input.x).normalized;

        footsteps.getPlaybackState(out pLAYBACK_STATE);

        if (mv.sqrMagnitude > .01f)
        {
            controller.Move(stateMachine.normalSpeed * Time.fixedDeltaTime * mv);
        }
    }

    public override void HandleInput()
    {
        if (stateMachine.MovementInput.sqrMagnitude <= .01f)
            stateMachine.ChangeState(new State_Idle(stateMachine));

        if (stateMachine.MovementInput.sqrMagnitude > .01f && stateMachine.SprintInput && stateMachine.currentStamina >= 35)
            stateMachine.ChangeState(new State_Sprint(stateMachine));

        if (stateMachine.HidingInput && CheckHidingPosition())
            stateMachine.ChangeState(new State_Hiding(stateMachine));
    }

    public override void ExitState()
    {
        footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
