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
    Animations.Add(WalkRight.Name, WalkRight);
    Animations.Add(WalkBottom.Name, WalkBottom);
    Animations.Add(WalkLeft.Name, WalkLeft);

    ConfirmAnimations();
    HideAllAnimations();
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    switch (Entity.LastCommandDirection)
    {
      case DIRECTIONS.TOP:
        PlayAnimation(WalkTop);
        break;
      case DIRECTIONS.RIGHT:
        PlayAnimation(WalkRight);
        break;
      case DIRECTIONS.BOTTOM:
        PlayAnimation(WalkBottom);
        break;
      case DIRECTIONS.LEFT:
        PlayAnimation(WalkLeft);
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

  public AnimationData WalkLeft
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Left",
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

  public AnimationData WalkRight
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Right",
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
