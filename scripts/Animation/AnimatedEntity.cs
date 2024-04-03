using Godot;
using Entity;
using System;


namespace Animation;

public abstract partial class AnimatedEntity(Vector2 initialPosition) : Entity.Entity(initialPosition)
{
  public abstract override StringName ResourceName { get; set; }

  public override void _Process(double delta)
  {
    base._Process(delta);
    QueueMovementStateAnimations();
  }

  public void QueueMovementStateAnimations()
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