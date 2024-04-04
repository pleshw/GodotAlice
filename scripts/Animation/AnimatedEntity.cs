using Godot;
using Entity;
using System;


namespace Animation;

public abstract partial class AnimatedEntity(Vector2 initialPosition) : Entity.Entity(initialPosition)
{
  public abstract override StringName ResourceName { get; set; }
  public AnimationData IdleState;
  public EntityAnimationInfo AnimationState;

  public override void _Ready()
  {
    base._Ready();

    foreach (var item in _animationsByName)
    {
      item.Value.AnimationFinished += () => AnimationState.OnAnimationFinished(item.Value);
    }

    AnimationState = EntityAnimationInfo.GetInfoFrom(this);

    MovementStateUpdated += UpdateAnimation;

    MovementInputTriggered += () =>
    {
      UpdateAnimation();
    };
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }



  public void UpdateAnimation()
  {
    switch (MovementController.MovementState)
    {
      case MOVEMENT_STATE.WALKING:
        movementAnimator.Play();
        break;
      case MOVEMENT_STATE.IDLE:
        idleAnimator.Play();
        break;
      default:
        return;
    }
  }
}