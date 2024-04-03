using Godot;
using Entity;
using System;


namespace Animation;

public abstract partial class AnimatedEntity : Entity.Entity
{
  public abstract override StringName ResourceName { get; set; }

  public EntityAnimationInfo AnimationInfo;

  public AnimatedEntity(Vector2 initialPosition) : base(initialPosition)
  {
    AnimationInfo = AnimationMediator.GetInfo(this);
  }

  public override void _Ready()
  {
    base._Ready();
    foreach (var item in Animations)
    {
      item.Value.AnimationFinished += AnimationInfo.OnAnimationFinished;
    }
  }

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
        if (AnimationInfo.CanUpdateAnimation)
        {
          movementAnimator.Play();
        }
        break;
      case MOVEMENT_STATE.IDLE:
        if (AnimationInfo.CanUpdateAnimation && AnimationInfo.AnimationPlaying.Animation != idleAnimator.IdleAnimationData.Animation)
        {
          idleAnimator.Play();
        }
        break;
      default:
        return;
    }
  }
}