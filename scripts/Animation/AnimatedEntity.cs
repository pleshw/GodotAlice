using Godot;
using Entity;
using System;
using GameManagers;
using System.Collections.Generic;
using Animation;


namespace Entity;

public abstract partial class AnimatedEntity(Vector2 initialPosition) : Entity(initialPosition)
{
  protected Dictionary<StringName, AnimatedBody> _animationsByName = [];
  public Dictionary<StringName, AnimationData> Animations { get; set; } = [];

  public EntityIdleAnimator idleAnimator;
  public EntityMovementAnimator movementAnimator;
  public EntityAttackAnimator attackAnimator;
  public AnimationData IdleState;
  public EntityAnimationInfo AnimationState;

  public override void Setup(Vector2 initialPosition = default, int gridCellWidth = 32)
  {
    base.Setup(initialPosition, gridCellWidth);

    movementAnimator = new EntityMovementAnimator(this);
    idleAnimator = new EntityIdleAnimator(this);
    attackAnimator = new EntityAttackAnimator(this);
  }

  public override void _Ready()
  {
    base._Ready();

    AddAnimationSprites(IdleSpritesByName);
    AddAnimationSprites(MovementRelatedSpritesByName);

    idleAnimator.OnReady();
    movementAnimator.OnReady();
    attackAnimator.OnReady();

    AnimationState = EntityAnimationInfo.GetInfoFrom(this);

    idleAnimator.Play();

    OnEntityStoppedEvent += () =>
    {
    };

    OnEntityMovedEvent += (Vector2 from, Vector2 to) =>
    {
    };
  }

  public override void _Process(double delta)
  {
    base._Process(delta);
  }


  public void CancelCurrentAction()
  {
    if (!GameStates.HasFlag(GameStates.PERFORMING_ACTION))
    {
      return;
    }

    GameStates &= ~GameStates.PERFORMING_ACTION;
    MovementController.CancelOnNextIteration = true;
  }
}