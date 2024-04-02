using Godot;

namespace Animation;

public abstract class Animator(Entity.Entity entity)
{
  protected Entity.Entity _entity = entity;
  public abstract int Priority { get; set; }

  public EntityAnimationInfo AnimationInfo
  {
    get
    {
      return AnimationMediator.GetInfo(_entity);
    }
  }

  public AnimatedSprite2D LastPlayedAnimation
  {
    get
    {
      return AnimationInfo.LastPlayedAnimation;
    }
  }

  public bool TryPlayAnimation(AnimatedSprite2D animationToPlay, StringName animationName)
  {
    return AnimationMediator.TryPlayAnimationOn(new PlayAnimationRequest
    {
      Entity = _entity,
      Animator = this,
      Animation = animationToPlay,
      AnimationName = animationName
    }, out _);
  }

  public abstract void HideAllAnimations();
}