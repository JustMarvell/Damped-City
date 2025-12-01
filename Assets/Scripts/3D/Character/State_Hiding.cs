using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hiding : CharacterState_3D
{
    public State_Hiding(StateMachine_3D stateMachine) : base(stateMachine) { }

    Transform hidingPosition;
    Vector3 initialPosition;
    Quaternion initialRotation;

    C_CameraController _camera;

    public float rotX;
    public float rotY;

    public override void EnterState()
    {
        stateMachine.m_currentState = CURRENT_STATE_3D.HIDING;

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
        transform.SetPositionAndRotation(initialPosition, initialRotation);

        _camera.TriggerUnhide();
        _camera.ResetAngleClamp();

        stateMachine.controller.enabled = true;
        stateMachine.bc.enabled = true;
    }
}
