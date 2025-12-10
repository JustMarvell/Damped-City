using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class State_Hiding : CharacterState_3D
{
    public State_Hiding(StateMachine_3D stateMachine) : base(stateMachine) { }

    Transform hidingPosition;
    Vector3 initialPosition;
    Quaternion initialRotation;

    C_CameraController _camera;

    public float rotX;
    public float rotY;

    PLAYBACK_STATE hidingPlaybackEnterState;
    PLAYBACK_STATE hidingPlaybackExitState;

    public override void EnterState()
    {
        stateMachine.m_currentState = CURRENT_STATE_3D.HIDING;

        hidingEnter.getPlaybackState(out hidingPlaybackEnterState);
        hidingExit.getPlaybackState(out hidingPlaybackExitState);

        if (hidingPlaybackEnterState.Equals(PLAYBACK_STATE.STOPPED))
        {
            hidingEnter.start();
            hidingExit.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        _camera = C_CameraController.instance;

        hidingPosition = GetHidingPositionAndRotation();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        stateMachine.controller.enabled = false;
        stateMachine.bc.enabled = false;

        transform.SetPositionAndRotation(hidingPosition.GetChild(0).position, hidingPosition.localRotation);
        
        _camera.SetNewAngleClamp(stateMachine.hidingAngleClamp);
        _camera.TriggerHiding(hidingPosition.localRotation);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void HandleInput()
    {
        if (stateMachine.HidingInput)
        {
            stateMachine.ChangeState(new State_Idle(stateMachine));
        }
    }

    public override void ExitState()
    {
        hidingEnter.getPlaybackState(out hidingPlaybackEnterState);
        hidingExit.getPlaybackState(out hidingPlaybackExitState);

        if (hidingPlaybackExitState.Equals(PLAYBACK_STATE.STOPPED))
        {
            hidingExit.start();
            hidingEnter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        transform.SetPositionAndRotation(initialPosition, initialRotation);

        _camera.TriggerUnhide();
        _camera.ResetAngleClamp();

        stateMachine.controller.enabled = true;
        stateMachine.bc.enabled = true;
    }
}
