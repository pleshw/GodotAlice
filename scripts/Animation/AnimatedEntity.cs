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

    AnimationState = EntityAnimationInfo.GetInfoFrom(this);

    EntityMoved += (Vector2 from, Vector2 to) =>
    {
      movementAnimator.Play();
    };
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }
}