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

  private AnimationData _walkTop;
  private AnimationData _walkBottom;
  private AnimationData _walkLeft;
  private AnimationData _walkRight;


  public AnimationData WalkTop { get { return _walkTop; } }
  public AnimationData WalkBottom { get { return _walkBottom; } }
  public AnimationData WalkLeft { get { return _walkLeft; } }
  public AnimationData WalkRight { get { return _walkRight; } }

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

    Animations.Add(WalkTopData.Name, WalkTop);
    Animations.Add(WalkRightData.Name, WalkRight);
    Animations.Add(WalkBottomData.Name, WalkBottom);
    Animations.Add(WalkLeftData.Name, WalkLeft);


    _walkTop = WalkTopData;
    _walkBottom = WalkBottomData;
    _walkLeft = WalkLeftData;
    _walkRight = WalkRightData;

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
        PlayAnimation(WalkTopData);
        break;
      case DIRECTIONS.RIGHT:
        PlayAnimation(WalkRightData);
        break;
      case DIRECTIONS.BOTTOM:
        PlayAnimation(WalkBottomData);
        break;
      case DIRECTIONS.LEFT:
        PlayAnimation(WalkLeftData);
        break;
      default:
        PlayAnimation(Entity.idleAnimator.Idle);
        break;
    }
  }

  private AnimationData WalkTopData
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Top",
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

  private AnimationData WalkBottomData
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Bottom",
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

  private AnimationData WalkLeftData
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Left",
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

  private AnimationData WalkRightData
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Walking"],
        Name = "Right",
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
