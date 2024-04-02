using System;
using System.Collections.Generic;
using Godot;

namespace Animation;

public class EntityAnimationInfo
{
  public Animator AnimatorPlaying { get; set; } = null;
  public AnimatedSprite2D AnimationPlaying { get; set; } = null;

  public Animator LastPlayedAnimator { get; set; } = null;
  public AnimatedSprite2D LastPlayedAnimation { get; set; } = null;

  public void StopCurrentAnimation()
  {
    if (AnimatorPlaying != null)
    {
      LastPlayedAnimator = AnimatorPlaying;
      AnimatorPlaying = null;
    }

    if (AnimationPlaying != null)
    {
      AnimationPlaying.Pause();
      AnimationPlaying.Frame = 0;
      AnimationPlaying.Visible = false;
      LastPlayedAnimation = AnimationPlaying;
    }

    AnimationPlaying = null;
  }

  public void PlayAnimation(Animator animatorPlaying, AnimatedSprite2D animationToPlay, StringName animationName)
  {
    AnimatorPlaying = animatorPlaying;
    AnimationPlaying = animationToPlay;

    AnimationPlaying.Visible = true;
    AnimationPlaying.Play(animationName);
  }

  public void SwitchAnimation(Animator animatorPlaying, AnimatedSprite2D animationToPlay, StringName animationName)
  {
    StopCurrentAnimation();
    PlayAnimation(animatorPlaying, animationToPlay, animationName);
  }
}