using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public class EntityAnimationInfo
{
  private static readonly Dictionary<Guid, EntityAnimationInfo> entityAnimationInfoById = [];

  public required AnimationData DefaultAnimationData;
  public AnimatorNode MainAnimator { get; set; } = null;
  public AnimationData MainAnimationData { get; set; } = null;
  public AnimationData ConcurrentAnimations { get; set; } = null;
  public readonly EntityAnimationController Controller;


  public SortedSet<AnimationData> AnimationSet = new(Comparer<AnimationData>.Create((a, b) => a.Priority.CompareTo(b.Priority)));

  public EntityAnimationInfo()
  {
    Controller = new(this);
  }


  public static EntityAnimationInfo GetInfoFrom(Entity.Entity entity)
  {
    if (entityAnimationInfoById.TryGetValue(entity.Id, out EntityAnimationInfo animInfo))
    {
      return animInfo;
    }

    EntityAnimationInfo animationInfo = new()
    {
      DefaultAnimationData = entity.idleAnimator.IdleAnimationData
    };

    entityAnimationInfoById.Add(entity.Id, animationInfo);

    return GetInfoFrom(entity);
  }

  public static bool HaveHigherPriority(AnimationData a1, AnimationData a2)
  {
    if (a1 == null)
    {
      return false;
    }

    if (a2 == null)
    {
      return true;
    }

    if (a1.Priority > a2.Priority)
    {
      return true;
    }

    return false;
  }

  public static bool HaveSamePriority(AnimationData a1, AnimationData a2)
  {
    if (a1 == null)
    {
      return false;
    }

    if (a2 == null)
    {
      return true;
    }

    if (a1.Priority == a2.Priority)
    {
      return true;
    }

    return false;
  }

  public static bool HaveLessPriority(AnimationData a1, AnimationData a2)
  {
    if (a1 == null)
    {
      return true;
    }

    if (a2 == null)
    {
      return false;
    }

    if (a1.Priority < a2.Priority)
    {
      return true;
    }

    return false;
  }
}