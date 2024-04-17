using System.Collections.Generic;
using Entity;
using Godot;

namespace Animation;

public abstract partial class AnimatorNode : Node
{
  public static readonly Dictionary<AnimatedEntity, List<AnimatorNode>> ObserversByEntity = [];
  protected AnimatedEntity _entity;

  protected Dictionary<string, AnimatedBody> AnimationSprites = [];
  protected abstract Dictionary<string, AnimationData> Animations { get; set; }

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

  public EntityAnimationInfo AnimationInfo
  {
    get
    {
      return EntityAnimationInfo.GetInfoFrom(_entity);
    }
  }

  public void PlayAnimation(AnimationData animationData)
  {
    EntityAnimationController.RequestPlayAnimation(new PlayAnimationRequest
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

  public abstract void Play();

  public void ConfirmAnimations()
  {
    foreach (var anim in Animations)
    {
      _entity.Animations.Add(anim.Key, anim.Value);
    }
  }

  [Signal]
  public delegate void AnimationEndedEventHandler(AnimatedSprite2D animation);

  [Signal]
  public delegate void AnimationStartedEventHandler();
}