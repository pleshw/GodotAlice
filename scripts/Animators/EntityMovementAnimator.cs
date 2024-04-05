using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityMovementAnimator(Entity entity) : AnimatorNode(entity)
{
  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override void OnReady()
  {
    AddMovementAnimation("Walking");
    AddMovementAnimation("Running");
    AddMovementAnimation("Dashing", 3, false);

    ConfirmAnimations();
    HideAllAnimations();
  }

  public void AddMovementAnimation(string animationName, int animationPriority = 1, bool animationCanBeInterrupted = true)
  {
    if (_entity.AnimationsByName.TryGetValue(animationName, out AnimatedSprite2D dashingAnimations))
    {
      AnimationSprites.Add(animationName, dashingAnimations);
      SpriteFrames animations = dashingAnimations.SpriteFrames;
      string[] animationNames = animations.GetAnimationNames();
      foreach (string name in animationNames)
      {
        Animations.Add(animationName + name, GetMovementAnimationData(animationName, name, animationPriority, animationCanBeInterrupted));
      }
    }
  }

  private AnimationData GetMovementAnimationData(string spritesKey, string animationName, int priority = 1, bool canBeInterrupted = true)
  {
    return new AnimationData
    {
      Sprites = AnimationSprites[spritesKey],
      Name = animationName,
      Animator = this,
      Priority = priority,
      Entity = Entity,
      CanPlayConcurrently = false,
      CanBeInterrupted = canBeInterrupted,
      BeforeAnimationStart = () =>
      {
        AnimationSprites[spritesKey].FlipH = Entity.FacingSide == DIRECTIONS.LEFT;
      }
    };
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    MOVEMENT_STATE movementState = Entity.MovementController.States.Max;
    switch (movementState)
    {
      case MOVEMENT_STATE.WALKING:
        PlayWalkAnimation();
        return;
      case MOVEMENT_STATE.DASHING:
        PlayDashAnimation();
        return;
    }
  }

  private void PlayDashAnimation()
  {
    PlayAnimation(Animations["DashingDefault"]);
  }

  private void PlayWalkAnimation()
  {
    switch (Entity.LastCommandDirection)
    {
      case DIRECTIONS.TOP:
        PlayAnimation(Animations["WalkingTop"]);
        break;
      case DIRECTIONS.RIGHT:
        PlayAnimation(Animations["WalkingRight"]);
        break;
      case DIRECTIONS.BOTTOM:
        PlayAnimation(Animations["WalkingBottom"]);
        break;
      case DIRECTIONS.LEFT:
        PlayAnimation(Animations["WalkingLeft"]);
        break;
      default:
        PlayAnimation(Entity.idleAnimator.Idle);
        break;
    }
  }


  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
