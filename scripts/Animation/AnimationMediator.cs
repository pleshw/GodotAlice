using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public class AnimationData
{
  public required AnimatorNode Animator;
  public required StringName Name;
  public required AnimatedSprite2D Animation;
  public required bool WaitToFinish;
}

public struct PlayAnimationRequest
{
  public required Entity.Entity Entity;
  public required AnimatorNode Animator;
  public required AnimationData AnimationData;
}

public static class AnimationMediator
{
  private static readonly Dictionary<Guid, EntityAnimationInfo> entityAnimationInfoById = [];

  public static EntityAnimationInfo GetInfo(Entity.Entity entity)
  {
    if (entityAnimationInfoById.TryGetValue(entity.Id, out EntityAnimationInfo animInfo))
    {
      return animInfo;
    }

    EntityAnimationInfo animationInfo = new();
    entityAnimationInfoById.Add(entity.Id, animationInfo);

    return GetInfo(entity);
  }

  public static void PlayAnimation(PlayAnimationRequest request, out EntityAnimationInfo animationInfo)
  {
    animationInfo = GetInfo(request.Entity);

    /// Nothing playing condition
    if (animationInfo.AnimationPlaying == null || animationInfo.CanUpdateAnimation)
    {
      animationInfo.SwitchAnimation(request.AnimationData);
      return;
    }

    bool isRequestHigherPriorityThanPlaying = HaveHigherPriority(request.Animator, animationInfo.AnimatorPlaying);
    if (isRequestHigherPriorityThanPlaying)
    {
      animationInfo.SwitchAnimation(request.AnimationData);
      return;
    }
  }

  public static void ForcePlayAnimation(PlayAnimationRequest request, out EntityAnimationInfo animationInfo)
  {
    animationInfo = GetInfo(request.Entity);
    animationInfo.SwitchAnimation(request.AnimationData);
  }

  public static bool HaveHigherPriority(AnimatorNode a1, AnimatorNode a2)
  {
    if (a1 == null)
    {
      return false;
    }

    if (a2 == null)
    {
      return true;
    }

    if (a1.Priority >= a2.Priority)
    {
      return true;
    }

    return false;
  }
}