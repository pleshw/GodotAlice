using Godot;
using Entity;
using System;
using GameManagers;


namespace Animation;

public abstract partial class AnimatedEntity(Vector2 initialPosition) : Entity.Entity(initialPosition)
{
  public AnimationData IdleState;
  public EntityAnimationInfo AnimationState;

  public override void _Ready()
  {
    base._Ready();

    AnimationState = EntityAnimationInfo.GetInfoFrom(this);


    OnEntityStoppedEvent += () =>
    {
      idleAnimator.Play();
    };

    OnEntityMovedEvent += (Vector2 from, Vector2 to) =>
    {
      movementAnimator.Play();
    };
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }
}