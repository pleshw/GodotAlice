using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityIdleAnimator(Entity entity) : AnimatorNode(entity)
{
  public DIRECTIONS facingDirection = DIRECTIONS.BOTTOM;
  public DIRECTIONS lastFacingDirection = DIRECTIONS.BOTTOM;

  public override int Priority
  {
    get => 1;
    set { }
  }

  public AnimationData Idle
  {
    get
    {
      return new AnimationData
      {
        Animator = this,
        Animation = AnimationSprites["Idle"],
        Name = "Default",
        CanBeInterrupted = true,
        Priority = 1,
        Entity = Entity,
        CanPlayConcurrently = false,
        BeforeAnimationStart = () =>
        {
          IdleSprite2D.FlipH = Entity.facingDirection == DIRECTIONS.LEFT;
        }
      };
    }
  }

  public AnimatedSprite2D IdleSprite2D
  {
    get
    {
      return Idle.Animation;
    }
  }

  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override void OnReady()
  {
    if (_entity.AnimationsByName.TryGetValue("Idle", out AnimatedSprite2D idleAnimations))
    {
      AnimationSprites.Add("Idle", idleAnimations);
    }

    Animations.Add(Idle.Name, Idle);

    ConfirmAnimations();
    HideAllAnimations();
  }

  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    PlayAnimation(Idle);
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
