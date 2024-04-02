using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public class AnimationData
{
  public required AnimatorNode Animator;
  public required StringName Name;
  public required AnimatedSprite2D Animation;
  public AnimationData PlayNext;
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

    entityAnimationInfoById.Add(entity.Id, new());

    return GetInfo(entity);
  }

  public static void PlayAnimation(PlayAnimationRequest request, out EntityAnimationInfo animationInfo)
  {
    animationInfo = GetInfo(request.Entity);

    /// Nothing playing condition
    if (animationInfo.AnimationPlaying == null)
    {
      animationInfo.AnimationEnqueued = null;
      animationInfo.SwitchAnimation(request.AnimationData);
      return;
    }

    /// Over priority condition
    if (request.Animator.Priority >= animationInfo.AnimatorPlaying.Priority)
    {
      animationInfo.AnimationEnqueued = null;
      animationInfo.SwitchAnimation(request.AnimationData);
      return;
    }

    /// Empty queue condition
    if (animationInfo.AnimationEnqueued == null)
    {
      animationInfo.AnimationEnqueued = request.AnimationData;
      return;
    }

    /// Queue priority condition
    if (request.Animator.Priority >= animationInfo.AnimationEnqueued.Animator.Priority)
    {
      animationInfo.AnimationEnqueued = request.AnimationData;
      return;
    }
  }
}