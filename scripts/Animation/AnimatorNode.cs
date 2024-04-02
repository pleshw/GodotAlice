using System.Collections.Generic;
using Godot;

namespace Animation;

using AnimatedEntity = Entity.Entity;

public abstract partial class AnimatorNode : Node
{
  public static readonly Dictionary<AnimatedEntity, List<AnimatorNode>> ObserversByEntity = [];
  protected AnimatedEntity _entity;
  public AnimationData PlayNext { get; set; } = null;

  protected Dictionary<string, AnimatedSprite2D> AnimationSprites = [];

  public AnimatedEntity Entity
  {
    get
    {
      return _entity;
    }
  }

  public abstract void OnReady();

  public AnimatorNode(AnimatedEntity entity)
  {
    _entity = entity;
    AddObserver(_entity, this);
  }

  public List<AnimatorNode> Observers
  {
    get
    {
      return GetObserver(_entity);
    }
  }

  public abstract int Priority { get; set; }

  public EntityAnimationInfo AnimationInfo
  {
    get
    {
      return AnimationMediator.GetInfo(_entity);
    }
  }

  public AnimationData LastPlayedAnimation
  {
    get
    {
      return AnimationInfo.LastPlayedAnimation;
    }
  }

  public void TryPlayAnimation(AnimationData animationData)
  {
    AnimationMediator.PlayAnimation(new PlayAnimationRequest
    {
      Entity = _entity,
      Animator = this,
      AnimationData = animationData
    }, out _);
  }

  public static List<AnimatorNode> GetObserver(AnimatedEntity entity)
  {
    if (ObserversByEntity.TryGetValue(entity, out List<AnimatorNode> animators))
    {
      return animators;
    }

    ObserversByEntity.Add(entity, []);

    return GetObserver(entity);
  }

  public static List<AnimatorNode> AddObserver(AnimatedEntity entity, AnimatorNode animator)
  {
    if (ObserversByEntity.TryGetValue(entity, out List<AnimatorNode> animators))
    {
      animators.Add(animator);
      return animators;
    }

    ObserversByEntity.Add(entity, [animator]);

    return GetObserver(entity);
  }

  public abstract void HideAllAnimations();

  [Signal]
  public delegate void AnimationEndedEventHandler(AnimatedSprite2D animation);

  [Signal]
  public delegate void AnimationStartedEventHandler();
}