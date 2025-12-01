using UnityEngine;
using UnityEngine.InputSystem;


public class State
{
    protected StateMachine stateMachine;
    protected CharacterController controller;
    protected Animator animator;
    protected Transform transform;
    protected Camera mainCamera;

    protected State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.controller = stateMachine.Controller;
        this.animator = stateMachine.Animator;
        this.transform = stateMachine.Transform;
        this.mainCamera = Camera.main;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void HandleInput() { }
    public virtual bool CanJump() => stateMachine.IsGrounded();
}
