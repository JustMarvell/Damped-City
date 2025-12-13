using Cinemachine;
using FMOD.Studio;
using UnityEngine;

[System.Serializable]
public abstract class CharacterState_3D
{
    public StateMachine_3D stateMachine;
    public CharacterController controller;
    public Animator animator;
    public Transform transform;
    public Camera cam;
    public EventInstance footsteps;
    public EventInstance sprintsteps;
    public EventInstance hidingEnter;
    public EventInstance hidingExit;

    public CharacterState_3D(StateMachine_3D _statemachine)
    {
        stateMachine = _statemachine;
        controller = _statemachine.controller;
        animator = _statemachine.animator;
        transform = _statemachine.Transform;
        cam = Camera.main;

        footsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PLAYER_Footsteps);
        sprintsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PLAYER_Sprintsteps);
        hidingEnter = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PLAYER_HidingEnter);
        hidingExit = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PLAYER_HidingExit);
    }

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
    public virtual void FixedUpdateState()
    {
        
    }
    public virtual void HandleInput() {}

    public bool CheckHidingPosition()
    {
        return Physics.CheckSphere(transform.position, 3f, stateMachine.hidingPlaceMask);
    }

    public Transform GetHidingPositionAndRotation()
    {
        return Physics.OverlapSphere(transform.position, 3f, stateMachine.hidingPlaceMask)[0].transform;
    }

    public void StopPlayerSounds()
    {
        Debug.Log("Stoping player sounds");

        footsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        sprintsteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);        
    }
}
