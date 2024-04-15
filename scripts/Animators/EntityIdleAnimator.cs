using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityIdleAnimator(AnimatedEntity entity) : AnimatorNode(entity)
{
  public AnimationData IdleAnimationData
  {
    get
    {
      return Animations["IdleDefault"];
    }
  }

  public AnimatedSprite2D Idle
  {
    get
    {
      return AnimationSprites["Idle"];
    }
  }

  protected override Dictionary<string, AnimationData> Animations { get; set; } = [];

  public override void OnReady()
  {
    AddIdleAnimation("Idle");

    ConfirmAnimations();
    HideAllAnimations();
  }


  public void AddIdleAnimation(string animationName, int animationPriority = 1, bool animationCanBeInterrupted = true)
  {
    if (_entity.AnimationsByName.TryGetValue(animationName, out AnimatedSprite2D dashingAnimations))
    {
      AnimationSprites.Add(animationName, dashingAnimations);
      SpriteFrames animations = dashingAnimations.SpriteFrames;
      string[] animationNames = animations.GetAnimationNames();
      foreach (string name in animationNames)
      {
        Animations.Add(animationName + name, GetIdleAnimationData(animationName, name, animationPriority, animationCanBeInterrupted));
      }
    }
  }

  private AnimationData GetIdleAnimationData(string spritesKey, string animationName, int priority = 1, bool canBeInterrupted = true)
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
        AnimationSprites[spritesKey].FlipH = Entity.directionState.FacingSide == DIRECTIONS.LEFT;
      }
    };
  }


  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }

    PlayAnimation(IdleAnimationData);
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
