using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public partial class EntityMovementAnimator(Entity entity) : AnimatorNode(entity)
{
  public override int Priority
  {
    get => 2;
    set { }
  }

  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override void OnReady()
  {
    if (_entity.AnimationsByName.TryGetValue("Walking", out AnimatedSprite2D walkingAnimations))
    {
      AnimationSprites.Add("Walking", walkingAnimations);
    }

    if (_entity.AnimationsByName.TryGetValue("Running", out AnimatedSprite2D runningAnimations))
    {
      AnimationSprites.Add("Running", runningAnimations);
    }

    Animations.Add(WalkTop.Name, WalkTop);
    Animations.Add(WalkSides.Name, WalkSides);
    Animations.Add(WalkBottom.Name, WalkBottom);

    ConfirmAnimations();
    HideAllAnimations();
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    switch (Entity.FacingDirection)
    {
      case DIRECTIONS.TOP:
        PlayAnimation(WalkTop);
        break;
      case DIRECTIONS.RIGHT:
        PlayAnimation(WalkSides);
        break;
      case DIRECTIONS.BOTTOM:
        PlayAnimation(WalkBottom);
        break;
      case DIRECTIONS.LEFT:
        PlayAnimation(WalkSides);
        break;
      default:
        PlayAnimation(Entity.idleAnimator.Idle);
        break;
    }
  }

  public AnimationData WalkTop
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Top",
        CanBeInterrupted = true,
        Priority = 1,
        Entity = Entity,
        CanPlayConcurrently = false,
        BeforeAnimationStart = () =>
        {
          AnimationSprites["Walking"].FlipH = Entity.FacingSide == DIRECTIONS.LEFT;
        }
      };
    }
  }

  public AnimationData WalkBottom
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Bottom",
        CanBeInterrupted = true,
        Priority = 1,
        Entity = Entity,
        CanPlayConcurrently = false,
        BeforeAnimationStart = () =>
        {
          AnimationSprites["Walking"].FlipH = Entity.FacingSide == DIRECTIONS.LEFT;
        }
      };
    }
  }

  public AnimationData WalkSides
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Sides",
        CanBeInterrupted = true,
        Priority = 1,
        Entity = Entity,
        CanPlayConcurrently = false,
        BeforeAnimationStart = () =>
        {
          AnimationSprites["Walking"].FlipH = Entity.FacingSide == DIRECTIONS.LEFT;
        }
      };
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
