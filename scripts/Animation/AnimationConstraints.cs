
using System;
using Entity;
using Godot;

namespace Animation;

public class AnimationState
{
  public required int CurrentLoopTimes = 0;
}

public record class AnimationData
{
  public required StringName Name;
  public required int Priority;
  public bool CanBeInterrupted = true;
  public required Entity.Entity Entity;
  public required AnimatorNode Animator;
  public required AnimatedBody BodySprites;
  public required bool CanPlayConcurrently = false;
  public Action BeforeAnimationStart = () => { };

  /// <summary>
  /// Animation that concatenates after this end;
  /// </summary>
  public AnimationData Sequel;
}

public struct PlayAnimationRequest
{
  public required AnimatedEntity Entity;
  public required AnimatorNode Animator;
  public required AnimationData AnimationData;
}