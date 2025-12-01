using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class State_Sprint : CharacterState_3D
{
    public State_Sprint(StateMachine_3D stateMachine) : base(stateMachine) { }

    Vector2 input;
    Vector3 cameraForward, cameraRight;

    PLAYBACK_STATE pLAYBACK_STATE;

    public override void EnterState()
    {
        stateMachine.m_currentState = CURRENT_STATE_3D.SPRINT;

        sprintsteps.getPlaybackState(out pLAYBACK_STATE);

        if (pLAYBACK_STATE.Equals(PLAYBACK_STATE.STOPPED))
        {
            sprintsteps.start();
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

        if (mv.sqrMagnitude > .01f)
        {
            controller.Move(stateMachine.sprintSpeed * Time.fixedDeltaTime * mv);
        }

        sprintsteps.getPlaybackState(out pLAYBACK_STATE);

        ConsumeStamina();

        if (stateMachine.currentStamina <= 5)
            stateMachine.ChangeState(new State_Idle(stateMachine));
    }

    void ConsumeStamina()
    {
        stateMachine.currentStamina = Mathf.MoveTowards(stateMachine.currentStamina, -30, stateMachine.staminaConsumtionRate * Time.fixedDeltaTime);
    }

    public override void HandleInput()
    {
        if (!stateMachine.SprintInput)
            stateMachine.ChangeState(new State_Idle(stateMachine));

        if (stateMachine.HidingInput && CheckHidingPosition())
            stateMachine.ChangeState(new State_Hiding(stateMachine));
    }

    public override void ExitState()
    {
        sprintsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
