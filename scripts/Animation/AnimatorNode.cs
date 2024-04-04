using System.Collections.Generic;
using Godot;

namespace Animation;


public abstract partial class AnimatorNode : Node
{
  public static readonly Dictionary<Entity.Entity, List<AnimatorNode>> ObserversByEntity = [];
  protected Entity.Entity _entity;

  protected Dictionary<string, AnimatedSprite2D> AnimationSprites = [];
  protected abstract Dictionary<string, AnimationData> Animations { get; set; }

  public Entity.Entity Entity
  {
    get
    {
      return _entity;
    }
  }

  public abstract void OnReady();

  public AnimatorNode(Entity.Entity entity)
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

  public static List<AnimatorNode> GetObserver(Entity.Entity entity)
  {
    if (ObserversByEntity.TryGetValue(entity, out List<AnimatorNode> animators))
    {
      return animators;
    }

    ObserversByEntity.Add(entity, []);

    return GetObserver(entity);
  }

  public static List<AnimatorNode> AddObserver(Entity.Entity entity, AnimatorNode animator)
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