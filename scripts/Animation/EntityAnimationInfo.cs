using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public class EntityAnimationInfo
{
  public bool CanUpdateAnimation = true;
  public AnimatorNode AnimatorPlaying { get; set; } = null;
  public AnimationData AnimationPlaying { get; set; } = null;

  public AnimatorNode LastPlayedAnimator { get; set; } = null;
  public AnimationData LastPlayedAnimation { get; set; } = null;

  public AnimationData AnimationEnqueued { get; set; } = null;

  public static void StopAnimation(AnimatedSprite2D animation)
  {
    animation.Pause();
    animation.Frame = 0;
    animation.Visible = false;
  }

  public void StopCurrentAnimation()
  {
    if (AnimationPlaying != null)
    {
      StopAnimation(AnimationPlaying.Animation);
      LastPlayedAnimation = AnimationPlaying;
    }

    if (AnimatorPlaying != null)
    {
      LastPlayedAnimator = AnimatorPlaying;
    }

    AnimatorPlaying = null;
    AnimationPlaying = null;
    CanUpdateAnimation = true;
  }

  public void PlayAnimation(AnimationData animationToPlay)
  {
    CanUpdateAnimation = false;
    AnimatorPlaying = animationToPlay.Animator;
    AnimationPlaying = animationToPlay;

    AnimationPlaying.Animation.Visible = true;
    AnimationPlaying.Animation.Play(AnimationPlaying.Name);
  }

  public void OnAnimationFinished()
  {
    CanUpdateAnimation = true;
  }

  public void SwitchAnimation(AnimationData animationToPlay)
  {
    StopCurrentAnimation();
    PlayAnimation(animationToPlay);
  }
}