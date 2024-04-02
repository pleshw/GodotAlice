using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public struct PlayAnimationRequest
{
  public required Entity.Entity Entity;
  public required Animator Animator;
  public required AnimatedSprite2D Animation;
  public required StringName AnimationName;
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

  public static bool TryPlayAnimationOn(PlayAnimationRequest request, out EntityAnimationInfo animationInfo)
  {
    animationInfo = GetInfo(request.Entity);

    if (animationInfo.AnimationPlaying != null && animationInfo.AnimatorPlaying.Priority > request.Animator.Priority)
    {
      return false;
    }

    animationInfo.SwitchAnimation(request.Animator, request.Animation, request.AnimationName);
    animationInfo.AnimationPlaying.Play(request.AnimationName);

    return true;
  }
}